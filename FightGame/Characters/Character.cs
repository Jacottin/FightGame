using System;

namespace FightGame.Characters {
    public abstract class Character {
        protected string _className;
        protected string _userName;
        protected int _lifePoints;
        protected int _defendPoints;
        protected int _defaultAttackPoints;
        protected int _attackPoints;
        protected int _damages;
        private bool _specialCapacityUsed;
        protected string _lastAction;
        
        
        public void Attack(Character other) {
            other._damages = _attackPoints;
        }

        public void Defend(Character other) {
            _defendPoints = other._defaultAttackPoints;
        }

        public void Update(Character other) {
            if (_specialCapacityUsed) {
                _SpecialCapacity(other);
                _specialCapacityUsed = false;
            }

            if (_damages > 0) {
                _lifePoints = _lifePoints + (_defendPoints - _damages);
            }
            _damages = 0;
            _defendPoints = 0;

        }

        public void SpecialCapacity() {
            _specialCapacityUsed = true;
        }

        public string getClassName() {
            return _className;
        }

        public string getUserName() {
            return _userName;
        }

        public int getLife() {
            return _lifePoints;
        }
        
        public string getLastAction() {
            return _lastAction;
        }
        
        public void setLastAction(string action) {
            _lastAction = action;
        }

        public void actionDisplay() {
            switch (_lastAction) {
                case "attack":
                    Console.WriteLine($"{_userName} attaque !");
                    break;
                case "defend":
                    Console.WriteLine($"{_userName} se défend !");
                    break;
                case "special":
                    Console.WriteLine($"{_userName} utilise sa capacité spéciale !");
                    break;
            }
        }

        protected abstract void _SpecialCapacity(Character other);
    }
}