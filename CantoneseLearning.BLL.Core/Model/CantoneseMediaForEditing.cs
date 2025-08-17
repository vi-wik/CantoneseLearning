using System.ComponentModel;
using System.Runtime.CompilerServices;
using viwik.CantoneseLearning.Model;

namespace viwik.CantoneseLearning.BLL.Core.Model
{
    public class CantoneseMediaForEditing: V_CantoneseMedia, INotifyPropertyChanged
    {
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isEditing;
        private bool isSelected;

        public bool IsEditing
        {
            get
            {
                return this.isEditing;
            }

            set
            {
                if (value != this.isEditing)
                {
                    this.isEditing = value;
                    NotifyPropertyChanged();
                }
            }
        }
        

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }
       
        public double Progress { get; set; }
    }
}
