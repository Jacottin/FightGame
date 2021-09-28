﻿using System;

namespace FightGame.Characters {
    public class Healer : Character {

        public Healer(string playerName) {
            _className = "Healer";
            _userName = playerName;
            _lifePoints = 4;
            _totalLifePoints = _lifePoints;
            _defendPoints = 0;
            _attackPoints = 1;
            _defaultAttackPoints = _attackPoints;
            _damages = 0;

        }
        
        
        protected override void _SpecialCapacity(Character other) {
            ++_lifePoints;
            _lifePoints = Math.Min(_totalLifePoints, _lifePoints);
        }
    }
}