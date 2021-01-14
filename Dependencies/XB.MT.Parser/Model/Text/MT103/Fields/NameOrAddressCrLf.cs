namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class NameOrAddressCrLf
    {
        public string NameOrAddress { get; set; }
        public string CarriageReturn { get; set; }
        public string LineFeed { get; set; }

        public NameOrAddressCrLf(string nameOrAdress, bool setCrLf)
        {
            NameOrAddress = nameOrAdress;
            if (setCrLf)
            {
                CarriageReturn = "\r";
                LineFeed = "\n";
            }
        }

        public override bool Equals(object o)
        {
            if (o == null || !GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                NameOrAddressCrLf other = (NameOrAddressCrLf)o;
                return (NameOrAddress == other.NameOrAddress &&
                        CarriageReturn == other.CarriageReturn &&
                        LineFeed == other.LineFeed);
            }
        }
        public override int GetHashCode()
        {
            return NameOrAddress.GetHashCode() + CarriageReturn.GetHashCode() + LineFeed.GetHashCode();
        }

    }
}
