using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using esstp.Models.ViewModels;

namespace esstp.Models.Services
{
    /// <summary>
    /// The 'Observer' abstract class
    /// </summary>
    public abstract class Observer
    {
        public abstract ObserverModel Update(Observer o);

        public abstract SubjectModel GetObserver();

    }


}