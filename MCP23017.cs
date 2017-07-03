using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

using System.Diagnostics;
using Windows.Devices.I2c;
using Windows.Devices.Spi;
using Windows.Devices.Gpio;
using Windows.Devices.Enumeration;


   public class MCP23017
        {
            private byte _adress;
            private I2cDevice _mcp;
            private byte portA, portB;


            #region Const bank = 0

            const byte _reg_DIRA = 0x00;    //  reg directions i/o
            const byte _reg_DIRB = 0x01;    //  0=output    1=input
            const byte _reg_POLA = 0x02;    //  reg polarity direct or inverse
            const byte _reg_POLB = 0x03;
            const byte _reg_PIOA = 0x12;    //  bit register
            const byte _reg_PIOB = 0x13;
            

            #endregion


            public MCP23017(byte _adr = 0x20)
            {
                _adress = _adr;
                start();
            }

            private async void start()
            {
                var sett = new I2cConnectionSettings(_adress);
                sett.BusSpeed = I2cBusSpeed.FastMode;
                string sel = I2cDevice.GetDeviceSelector();
                var disp = await DeviceInformation.FindAllAsync(sel);
                _mcp = await I2cDevice.FromIdAsync(disp[0].Id, sett);

                // all output
                byte[] _wb = new byte[2];
                _wb[0] = _reg_DIRA;
                _wb[1] = 0;
                _mcp.Write(_wb);

                _wb[0] = _reg_DIRB;
                _mcp.Write(_wb);
                // all output
            }


            public void SetPinA(byte _pin, bool _val)
            {
                if (_val == false) portA &= (byte)~(0x01 << _pin);
                else portA |= (byte)(0x01 << _pin);

                updPins();
            }

        

            private void updPins()
            {
                byte[] _wb = new byte[2];
                _wb[0] = _reg_PIOA;
                _wb[1] = portA;
                _mcp.Write(_wb);

                _wb[0] = _reg_PIOB;
                _wb[1] = portB;
                _mcp.Write(_wb);

            }



        }