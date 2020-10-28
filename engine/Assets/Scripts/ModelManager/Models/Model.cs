using System;
using UnityEngine;

namespace Synthesis.ModelManager.Models
{
    public abstract class Model
    {
        protected GameObject gameObject;

        public static implicit operator GameObject(Model model) => model.gameObject;
    }
}