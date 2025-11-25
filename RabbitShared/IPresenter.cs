using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitShared
{
    //ШЛАК
    public interface IPresenter
    {
        void Initialize();
        string[] GetBreeds();
    }
}
