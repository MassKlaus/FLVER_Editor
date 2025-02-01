using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions
{
    public class ToggleBackFacesAction : TransformAction
    {
        private readonly List<FLVER2.FaceSet> facesets;
        private readonly Action refresher;

        public ToggleBackFacesAction(List<FLVER2.FaceSet> facesets, Action refresher)
        {
            this.facesets = facesets;
            this.refresher = refresher;
        }

        public override void Execute()
        {
            foreach (FLVER2.FaceSet fs in facesets)
                fs.CullBackfaces = !fs.CullBackfaces;

            refresher.Invoke();
        }

        public override void Undo()
        {
            foreach (FLVER2.FaceSet fs in facesets)
                fs.CullBackfaces = !fs.CullBackfaces;

            refresher.Invoke();
        }
    }
}
