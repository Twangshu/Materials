using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudtian
{
    public interface IUpdater
    {
        void Update();
    }

    public interface ILateUpdater
    {
        void LateUpdate();
    }
}
