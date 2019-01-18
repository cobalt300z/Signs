using System.Linq;

namespace KPlayerDLL
{

    public class Signs
    {
        private const ushort LumCoeff = 330;

        private ushort _phcell;
        public ushort SenderId { get; set; }
        public int SignID { get; set; }
        public string LaneID { get; set; }
        public byte CurrentBrightness { get; set; }
        public byte ExpectedBrightness { get; set; }
        public int PhotocellLocation { get; set; }
        public bool PollingState { get; set; }
        public int BrightnessMode { get; set; }
        public ushort Lux
        {
            get
            {
                return (ushort)(_phcell * LumCoeff / 100);
            }
            set
            {
                _phcell = value;
            }
        }

        public ReceiverCard[] recCard;

        public Signs()
        {
            recCard = new ReceiverCard[2];
            for (int i = 0; i < 2; i++)
            {
                recCard[i] = new ReceiverCard();
            }
        }

        public class ReceiverCard
        {

            public Modules[] Mod;
            private float _recTemp;
            private float _recvolt;
            private byte _recCardprops;

            public ReceiverCard()
            {
                Mod = new Modules[18];
                for (int i = 0; i < Mod.Count(); i++)
                {
                    Mod[i] = new Modules();
                }
            }

            public ushort PhotoCell { get; set; }
            public int RecCardIndex { get; set; }

            public float RecVolt
            {
                get
                {
                    return _recvolt;
                }
                set
                {
                    _recvolt = (value / 1000f);
                }
            }
            public ushort BadPixels { get; set; }

            public float RecTemp
            {
                get
                {
                    return _recTemp;
                }
                set
                {
                    _recTemp = (value / 10.00f);
                }
            }
            public byte RecCardProps
            {
                set
                {
                    _recCardprops = value;
                }
            }

            public bool RecCardMode
            {
                get
                {
                    return (_recCardprops & 0x1) != 0;
                }
            }
        }
        public class Modules
        {
            public byte ColAddr { get; set; }
            public byte RowAddr { get; set; }

            private float _modVolt;

            public float ModVolt
            {
                get
                {
                    return _modVolt;
                }

                set
                {
                    _modVolt = (float)(value * 32.00 / 1000);
                }
            }
            public sbyte ModTemp { get; set; }
            public bool CableStatus { get; set; }



        }

    }
}
