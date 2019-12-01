using System;
using esstp.Models.ViewModels;

namespace esstp.Models.Services
{
  

    public class ConcreteObserver : Observer
    {


        private SubjectModel _obs; //class property;
        private object _observerState;
        private ConcreteSubject _subject; //class property;
        

        // Constructor
        public ConcreteObserver(
      
          ConcreteSubject subject,
          SubjectModel obs)
        {
            this._subject = subject;
            this._obs = obs;

        }


        public override SubjectModel GetObserver()
        {
            return _obs;
        }


        public override ObserverModel Update(Observer o)
        {
            _observerState = _subject.SubjectState;

            SubjectModel sub = (SubjectModel)_observerState;

            SubjectModel obs = _obs;

          

            Console.WriteLine("Calling update...");

            ObserverModel om = new ObserverModel();

            try
            {
               
                if (obs.Symbol == sub.Symbol)
                {


                        //cfd type
                        switch (obs.Cfd_Id)
                        {
                            case 1:
                                Console.WriteLine("Invoke update Observer {0}'s new state of {1} close Buying",
                                    obs.Id, sub.Symbol);

                                if (sub.Buying >= obs.TakeProfit)
                                {
                                
                                    om.Id = Convert.ToInt32(obs.Id);
                                    om.Action = "TakeProfit";
                                    om.O = o;
                                   
                            }
                                if (sub.Buying <= obs.StopLoss)
                                {
                                    om.Id = Convert.ToInt32(obs.Id);
                                    om.Action = "StopLoss";
                                    om.O = o;
                            }
                                break;
                            case 2:
                                Console.WriteLine("Invoke update Observer {0}'s new state of {1} close Selling",
                                     obs.Id, sub.Symbol);
                                if (sub.Selling <= obs.TakeProfit)
                                {
                                    om.Id = Convert.ToInt32(obs.Id);
                                    om.Action = "TakeProfit";
                                    om.O = o;
                            }
                                if (sub.Selling >= obs.StopLoss)
                                {
                                    om.Id = Convert.ToInt32(obs.Id);
                                    om.Action = "StopLoss";
                                    om.O = o;
                                }
                                break;
                            default:
                                Console.WriteLine("Other");
                                break;
                        }


                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e);
            }
            Console.WriteLine("Returning update...{0}, {1}", om.Id, om.Action);
            return om;
        }





    }




}

