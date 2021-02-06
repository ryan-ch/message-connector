using XB.MT.Parser.Model.Common;

namespace XB.MT.Parser.Model.Text.MT103.Fields
{
    public class AdditionalRow
    {
        public string Data { get; set; }
        public string CarriageReturn { get; set; }
        public string LineFeed { get; set; }

        public AdditionalRow(string additionalRow, bool setCrLf)
        {
            Data = additionalRow;
            if (setCrLf)
            {
                CarriageReturn = Constants.Cr;
                LineFeed = Constants.Lf;
            }
        }

        public override bool Equals(object o)
        {
            if (o == null || !this.GetType().Equals(o.GetType()))
            {
                return false;
            }
            else
            {
                AdditionalRow other = (AdditionalRow)o;
                return (Data == other.Data &&
                        CarriageReturn == other.CarriageReturn &&
                        LineFeed == other.LineFeed);
            }
        }
        public override int GetHashCode()
        {
            return Data.GetHashCode() + CarriageReturn.GetHashCode() + LineFeed.GetHashCode();
        }

    }
}
