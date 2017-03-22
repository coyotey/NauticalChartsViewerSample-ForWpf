using System;

namespace NauticalChartsViewer
{
    public class ColorItem : ObservableObject, ICloneable
    {
        private short r;
        private short g;
        private short b;

        public ColorItem() { }

        public ColorItem(string code, string token, short r, short g, short b)
        {
            Name = code;
            Token = token;
            R = r;
            G = g;
            B = b;
        }

        public string Name { get; set; }

        public string Token { get; set; }

        public short R
        {
            get { return r; }
            set
            {
                if (r != value)
                {
                    r = value;
                    RaisePropertyChanged("R");
                }
            }
        }

        public short G
        {
            get { return g; }
            set
            {
                if (g != value)
                {
                    g = value;
                    RaisePropertyChanged("G");
                }
            }
        }

        public short B
        {
            get { return b; }
            set
            {
                if (b != value)
                {
                    b = value;
                    RaisePropertyChanged("B");
                }
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
