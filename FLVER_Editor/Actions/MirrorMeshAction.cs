using SoulsFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FLVER_Editor.Actions
{
    public class MirrorMeshAction : TransformAction
    {
        private readonly IEnumerable<FLVER2.Mesh> targetMeshes;
        private readonly IEnumerable<FLVER.Dummy> targetDummies;
        private readonly TransformAxis axis;
        private readonly bool useWorldOrigin;
        private readonly bool vertexMode;
        private readonly Action refresher;
        private readonly float[] totals;

        public MirrorMeshAction(List<FLVER2.Mesh> targetMeshes, List<FLVER.Dummy> targetDummies, TransformAxis axis, bool useWorldOrigin, bool vertexMode, Action refresher)
        {
            this.targetMeshes = targetMeshes;
            this.targetDummies = targetDummies;
            this.axis = axis;
            this.useWorldOrigin = useWorldOrigin;
            this.vertexMode = vertexMode;
            this.refresher = refresher;
            this.totals = MeshHelpers.CalculateMeshCenter(targetMeshes, useWorldOrigin);
            ;
        }

        private Vector3 MirrorThing(Vector3 v, TransformAxis axis, IReadOnlyList<float> totals)
        {
            // center to world 
            v[(int)axis] -= !useWorldOrigin ? totals[(int)axis] : 0;
            // flip
            v[(int)axis] *= -1;
            // uncenter to world 
            v[(int)axis] += !useWorldOrigin ? totals[(int)axis] : 0;

            return v;
        }

        public void MirrorMesh()
        {
            foreach (FLVER.Vertex v in targetMeshes.SelectMany(i => i.Vertices))
            {
                v.Position = MirrorThing(v.Position, axis, totals);

                v.Normal[(int)axis] *= -1;

                if (v.Tangents.Count > 0)
                {
                    var tagent = v.Tangents[0];
                    tagent[(int)axis] *= -1;
                    tagent.W = 1;
                    v.Tangents[0] = tagent;
                }
            }
            foreach (FLVER.Dummy d in targetDummies)
            {
                if (vertexMode) d.Forward = MirrorThing(d.Forward, axis, totals);
                else d.Position = MirrorThing(d.Position, axis, totals);
            }

            ReverseFaceSetsAction action = new(targetMeshes.SelectMany(x => x.FaceSets).ToList(), () => { });
            action.Execute();
        }

        public override void Undo()
        {
            MirrorMesh();
            refresher.Invoke();
        }

        public override void Execute()
        {
            MirrorMesh();
            refresher.Invoke();
        }
    }
}
