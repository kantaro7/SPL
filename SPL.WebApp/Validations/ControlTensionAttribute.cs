namespace SPL.WebApp.Validations
{
    using System.ComponentModel.DataAnnotations;

    public class ControlTensionAttribute : ValidationAttribute
    {
        private readonly string Test;
        private readonly string AT;
        private readonly string BT;
        private readonly string Ter;
        private readonly int Pos;

        public ControlTensionAttribute(string test, string aT, string bT, string ter, int pos)
        {
            Test = test;
            AT = aT;
            BT = bT;
            Ter = ter;
            Pos = pos;
        }

        public override bool IsValid(object value) => Pos == 1
                ? Test == "ABT"
                    ? (AT != "Todas" || BT != "Todas") && (AT == "Todas" || BT == "Todas")
                    : Test != "ATT" || ((AT != "Todas" || Ter != "Todas") && (AT == "Todas" || Ter == "Todas"))
                : Pos == 2
                    ? Test == "ABT"
                                    ? (AT != "Todas" || BT != "Todas") && (AT == "Todas" || BT == "Todas")
                                    : Test == "ATT" || ((BT != "Todas" || Ter != "Todas") && (BT == "Todas" || Ter == "Todas"))
                    : Test == "ABT"
                || (Test == "ATT"
                                        ? (AT != "Todas" || Ter != "Todas") && (AT == "Todas" || Ter == "Todas")
                                        : (BT != "Todas" || this.Ter != "Todas") && (BT == "Todas" || Ter == "Todas"));
    }
}
