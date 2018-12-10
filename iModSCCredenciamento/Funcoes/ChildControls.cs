using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace iModSCCredenciamento.Funcoes
{
    class ChildControls
    {
        private List<object> lstChildren;

        public List<object> GetChildren(Visual p_vParent, int p_nLevel)
        {
            if (p_vParent == null)
            {
                throw new ArgumentNullException("Element {0} is null!", p_vParent.ToString());
            }

            lstChildren = new List<object>();

            GetChildControls(p_vParent, p_nLevel);

            return lstChildren;

        }

        private void GetChildControls(Visual p_vParent, int p_nLevel)
        {
            int nChildCount = VisualTreeHelper.GetChildrenCount(p_vParent);

            for (int i = 0; i <= nChildCount - 1; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(p_vParent, i);

                lstChildren.Add(v);

                if (VisualTreeHelper.GetChildrenCount(v) > 0)
                {
                    GetChildControls(v, p_nLevel + 1);
                }
            }
        }
    }
}
