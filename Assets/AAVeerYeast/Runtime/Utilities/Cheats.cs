using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VeerYeast
{
    /// <summary>
    /// Cheats.
    /// </summary>
    public class Cheats : MonoSingleton<Cheats>
    {
        /// <summary>
        /// The cheat delegate.
        /// </summary>
        public delegate string Exec(params string[] values);

        /// <summary>
        /// The cheats.
        /// </summary>
        private Dictionary<string, Exec> mCheats = new Dictionary<string, Exec>();

        void Start()
        {
            gameObject.hideFlags = HideFlags.HideAndDontSave;
        }

        /// <summary>
        /// Regists the cheat.
        /// </summary>
        public static bool RegistCheat(string name, Exec cheat)
        {
            return Instance.AddCheat(name, cheat);
        }

        /// <summary>
        /// Adds the cheat.
        /// </summary>
        private bool AddCheat(string name, Exec cheat)
        {
            if (mCheats.ContainsKey(name)) return false;

            mCheats.Add(name, cheat);

            return true;
        }

        /// <summary>
        /// Executes the cheat.
        /// </summary>
        /// <param name='name'>
        /// Name.
        /// </param>
        /// <param name='values'>
        /// Values.
        /// </param>
        public static string ExecuteCheat(string name, params string[] values)
        {
            return Cheats.Instance.ExecCheat(name, values);
        }

        /// <summary>
        /// Executes the cheat.
        /// </summary>
        /// <param name='cheatName'>
        /// Name.
        /// </param>
        /// <param name='values'>
        /// Values.
        /// </param>
        private string ExecCheat(string cheatName, params string[] values)
        {
            Exec cheat;
            if (mCheats.TryGetValue(cheatName, out cheat))
            {
                if (cheat != null)
                {
                    return cheat(values);
                }
            }
            return "***unknown command***";
        }
    }
}