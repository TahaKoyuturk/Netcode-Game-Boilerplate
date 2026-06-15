using System;

namespace Studio.Core.Save
{
    [Serializable]
    public abstract class SaveDataBase
    {
        public int Version = 1;

        public virtual void Migrate()
        {
        }
    }
}
