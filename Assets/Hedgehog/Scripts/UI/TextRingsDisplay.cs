﻿using Hedgehog.Core.Actors;
using UnityEngine;
using UnityEngine.UI;

namespace Hedgehog.UI
{
    /// <summary>
    /// Ring counter.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class TextRingsDisplay : BaseRingsDisplay
    {
        /// <summary>
        /// The text on which to show the ring amount.
        /// </summary>
        [Tooltip("The text on which to show the ring amount.")]
        public Text Text;

        /// <summary>
        /// The ring collector from which to get the ring amount.
        /// </summary>
        [Tooltip("The ring collector from which to get the ring amount.")]
        public RingCollector Target;

        /// <summary>
        /// The target animator.
        /// </summary>
        [Header("Animation")]
        [Tooltip("The target animator.")]
        public Animator Animator;
        
        /// <summary>
        /// Name of an Animator int set to the number of rings collected.
        /// </summary>
        [Tooltip("Name of an Animator int set to the number of rings collected.")]
        public string AmountInt;
        protected int AmountIntHash;

        public void Reset()
        {
            Text = GetComponent<Text>();
            Target = FindObjectOfType<RingCollector>();

            Animator = GetComponent<Animator>();
            AmountInt = "";
        }

        public void Start()
        {
            Animator = Animator ? Animator : GetComponent<Animator>();
            AmountIntHash = Animator == null ? 0 : Animator.StringToHash(AmountInt);
            if(Target != null) Target.OnAmountChange.AddListener(OnAmountChange);
        }

        public void OnAmountChange()
        {
            Display(Target.Rings);
        }

        public override void Display(int rings)
        {
            Text.text = rings.ToString();
        }

        public void Update()
        {
            if (Target == null) return;
            if (!Animator) return;
            if(AmountIntHash != 0) Animator.SetInteger(AmountIntHash, Target.Rings);
        }
    }
}