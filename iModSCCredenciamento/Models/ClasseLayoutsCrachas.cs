using System.Collections.ObjectModel;

namespace iModSCCredenciamento.Models
{
    public class ClasseLayoutsCrachas
    {
        public ObservableCollection<LayoutCracha> LayoutsCrachas = new ObservableCollection<LayoutCracha>();

        public class LayoutCracha
        {
            private int _LayoutCrachaID;
            private string _Nome;
            private string _LayoutCrachaGUIDGuid;
            private string _LayoutRPT;

            public int LayoutCrachaID
            {
                get { return _LayoutCrachaID; }

                set { _LayoutCrachaID = value; }
            }
            public string Nome
            {
                get { return _Nome; }

                set { _Nome = value; }
            }
            public string LayoutCrachaGUID
            {
                get { return _LayoutCrachaGUIDGuid; }

                set { _LayoutCrachaGUIDGuid = value; }
            }

            public string LayoutRPT
            {
                get { return _LayoutRPT; }

                set { _LayoutRPT = value; }
            }
        }
    }
}
