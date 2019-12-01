using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using esstp.Models.ViewModels;

namespace esstp.Models.Services
{
    /// <summary>
    /// The 'Subject' abstract class
    /// </summary>
    public abstract class Subject
    {
        private List<Observer> _observers = new List<Observer>();
        //private List<ObserverModel> _id = new List<ObserverModel>();

        public void Attach(Observer observer)
        {
            _observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            _observers.Remove(observer);
            Console.WriteLine("::: Subject Detach observer :::");
        }

        public List<ObserverModel> Notify()
        {
                List<ObserverModel> _id = new List<ObserverModel>();
                Console.WriteLine("notify count of observers calling update: {0}", _observers.Count);
                //notify all observers of new Buy/sell values
                foreach (Observer o in _observers)
                {
                //if CFD observer got closed by TP or SL then
                //add its PK to the list and return it to be processed
                Console.WriteLine("::: in the observer list ::: {0}", o.GetObserver().Id);
                    var r = o.Update(o);
                   
                    if (r.Id != 0)
                        _id.Add(r);
                }
                //return list of CDF to be closed
                return _id;

        }

        public List<Observer> GetObserversList()
        {
            return _observers;
        }
    }
}
