using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RabbitSharedMVP
{
    public interface IDTO
    {
        int Id { get; set; }
        string Name { get; set; }
        string Breed { get; set; }
        int Age { get; set; }
        int Weight { get; set; }
    }

    public class RabbitDTO : IDTO, INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _breed;
        private int _age;
        private int _weight;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Breed
        {
            get => _breed;
            set { _breed = value; OnPropertyChanged(); }
        }

        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(); }
        }

        public int Weight
        {
            get => _weight;
            set { _weight = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class SortOperationDTO
    {
        public int SortField { get; set; }
        public bool Ascending { get; set; }

        public SortOperationDTO(int sortField, bool ascending)
        {
            SortField = sortField;
            Ascending = ascending;
        }
        public SortOperationDTO() { }
    }
}
