using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mc21.svr_viewer.ViewModel
{

    public delegate void ChangedCheckboxHandler(object sender);

    public class Checkbox
    {
        /// <summary>
        /// наименование чекбокса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Признак выбора чекбокса
        /// </summary>
        public bool isChecked 
        {
            get { return ischecked; }
            set 
            {
                if (value != ischecked) 
                {
                    ischecked = value;
                    OnCheckboxChanged();
                }
            } 
        }
        private bool ischecked;

        /// <summary>
        /// Количество
        /// </summary>
        public int Amount { get; set; }

#region Событие изменение выбора чекбокса
        
        // событие
        public event ChangedCheckboxHandler CheckboxChanged;

        // запускатель
        protected virtual void OnCheckboxChanged() 
        {
            if (CheckboxChanged != null)
                CheckboxChanged(this);
        }
#endregion

#region Конструсторы

        public Checkbox()
        {
        }

        public Checkbox(string str)
        {
            this.Name = str;
            this.isChecked = true;
        }

#endregion

    }
}
