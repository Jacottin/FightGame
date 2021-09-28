using System;

namespace FightGame.Characters {
    public abstract class Character {
        protected string _className;
        protected string _userName;
        protected int _lifePoints;
        protected int _totalLifePoints;
        protected int _defendPoints;
        protected int _defaultAttackPoints;
        protected int _attackPoints;
        protected int _damages;
        private bool _specialCapacityUsed;
        private AttackType _lastAction;
        
        
        public void Attack(Character other) {
            other._damages = _attackPoints;
        }

        public void Defend(Character other) {
            _defendPoints = other._defaultAttackPoints;
        }

        public void SetActualDamage(int otherDamages)
        {
            _damages = otherDamages;
        }
        
        public void Update(Character other) {
            if (_specialCapacityUsed) {
                _SpecialCapacity(other);
                _specialCapacityUsed = false;
            }
        }

        public void ComputeDamages() {
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

        public int getTotalLife() {
            return _totalLifePoints;
        }
        
        public void setLastAction(AttackType action) {
            _lastAction = action;
        }

        public void actionDisplay() {
            switch (_lastAction) {
                case AttackType.Attack:
                    Console.WriteLine($"{_userName} attaque !");
                    break;
                case AttackType.Defend:
                    Console.WriteLine($"{_userName} se défend !");
                    break;
                case AttackType.Special:
                    Console.WriteLine($"{_userName} utilise sa capacité spéciale !");
                    break;
            }
        }

        protected abstract void _SpecialCapacity(Character other);
    }
}