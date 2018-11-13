using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iModSCCredenciamento.Models
{
    public class ClasseStatus
    {
        public ObservableCollection<Status> Statuss = new ObservableCollection<Status>();

        public class Status
        {
            private int _StatusID;
            private string _Descricao;
            public int StatusID
            {
                get { return _StatusID; }

                set { _StatusID = value; }
            }
            public string Descricao
            {
                get { return _Descricao; }

                set { _Descricao = value; }
            }
        }
    }
}
