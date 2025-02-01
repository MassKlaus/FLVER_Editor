using SoulsFormats;

namespace FLVERS.Tests;


public static class FlverTestHelper
{
    public static void Equal(FLVER.Dummy expected, FLVER.Dummy actual)
    {
        Assert.Equal(expected.Position.X, actual.Position.X, 0.001);
        Assert.Equal(expected.Position.Y, actual.Position.Y, 0.001);
        Assert.Equal(expected.Position.Z, actual.Position.Z, 0.001);
        Assert.Equal(expected.Forward, actual.Forward);
        Assert.Equal(expected.Upward, actual.Upward);
        Assert.Equal(expected.ReferenceID, actual.ReferenceID);
        Assert.Equal(expected.ParentBoneIndex, actual.ParentBoneIndex);
        Assert.Equal(expected.AttachBoneIndex, actual.AttachBoneIndex);
        Assert.Equal(expected.Color, actual.Color);
        Assert.Equal(expected.Flag1, actual.Flag1);
        Assert.Equal(expected.UseUpwardVector, actual.UseUpwardVector);
        Assert.Equal(expected.Unk30, actual.Unk30);
        Assert.Equal(expected.Unk34, actual.Unk34);
    }

    public static void Equal(FLVER2 expected, FLVER2 actual)
    {
        Assert.Equal(expected.Compression, actual.Compression);
        Assert.Equal(expected.Header.BigEndian, actual.Header.BigEndian);
        Assert.Equal(expected.Header.BoundingBoxMax, actual.Header.BoundingBoxMax);
        Assert.Equal(expected.Header.BoundingBoxMin, actual.Header.BoundingBoxMin);
        Assert.Equal(expected.Header.Unicode, actual.Header.Unicode);
        Assert.Equal(expected.Header.Unk4A, actual.Header.Unk4A);
        Assert.Equal(expected.Header.Unk4C, actual.Header.Unk4C);
        Assert.Equal(expected.Header.Unk5C, actual.Header.Unk5C);
        Assert.Equal(expected.Header.Unk5D, actual.Header.Unk5D);
        Assert.Equal(expected.Header.Unk68, actual.Header.Unk68);
        Assert.Equal(expected.Header.Unk74, actual.Header.Unk74);
        Assert.Equal(expected.Header.Version, actual.Header.Version);

        Assert.Equal(expected.BufferLayouts.Count, actual.BufferLayouts.Count);
        Assert.Equal(expected.Dummies.Count, actual.Dummies.Count);
        Assert.Equal(expected.GXLists.Count, actual.GXLists.Count);
        Assert.Equal(expected.Materials.Count, actual.Materials.Count);
        Assert.Equal(expected.Meshes.Count, actual.Meshes.Count);
        Assert.Equal(expected.Nodes.Count, actual.Nodes.Count);
        Assert.Equal(expected.Skeletons.AllSkeletons.Count, actual.Skeletons.AllSkeletons.Count);
        Assert.Equal(expected.Skeletons.BaseSkeleton.Count, actual.Skeletons.BaseSkeleton.Count);

        for (int i = 0; i < expected.BufferLayouts.Count; i++)
        {
            var originalBuffer = expected.BufferLayouts[i];
            var buffer = actual.BufferLayouts[i];

            Assert.Equal(originalBuffer.Count, buffer.Count);

            for (int j = 0; j < originalBuffer.Count; j++)
            {
                var originalMember = originalBuffer[j];
                var member = buffer[j];

                Assert.Equal(originalMember.Index, member.Index);
                Assert.Equal(originalMember.Semantic, member.Semantic);
                Assert.Equal(originalMember.Size, member.Size);
                Assert.Equal(originalMember.Type, member.Type);
                Assert.Equal(originalMember.Unk00, member.Unk00);
            }
        }

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            var originalDummy = expected.Dummies[i];
            var dummy = actual.Dummies[i];

            Equal(originalDummy, dummy);
        }

        for (int i = 0; i < expected.GXLists.Count; i++)
        {
            var originalList = expected.GXLists[i];
            var list = actual.GXLists[i];

            Assert.Equal(originalList.Count, list.Count);
            for (int j = 0; j < originalList.Count; j++)
            {
                var originalItem = originalList[j];
                var item = list[j];

                Assert.Equal(originalItem.Data, item.Data);
                Assert.Equal(originalItem.ID, item.ID);
                Assert.Equal(originalItem.Unk04, item.Unk04);
            }
        }

        for (int i = 0; i < expected.Materials.Count; i++)
        {
            var originalMaterial = expected.Materials[i];
            var material = actual.Materials[i];

            Assert.Equal(originalMaterial.GXIndex, material.GXIndex);
            Assert.Equal(originalMaterial.MTD, material.MTD);
            Assert.Equal(originalMaterial.Index, material.Index);
            Assert.Equal(originalMaterial.Name, material.Name);
            Assert.Equal(originalMaterial.Textures.Count, material.Textures.Count);

            for (int j = 0; j < originalMaterial.Textures.Count; j++)
            {
                var originalTexture = originalMaterial.Textures[j];
                var texture = originalMaterial.Textures[j];

                Assert.Equal(originalTexture.Path, texture.Path);
                Assert.Equal(originalTexture.Scale, texture.Scale);
                Assert.Equal(originalTexture.Type, texture.Type);
                Assert.Equal(originalTexture.Unk10, texture.Unk10);
                Assert.Equal(originalTexture.Unk11, texture.Unk11);
                Assert.Equal(originalTexture.Unk14, texture.Unk14);
                Assert.Equal(originalTexture.Unk18, texture.Unk18);
                Assert.Equal(originalTexture.Unk1C, texture.Unk1C);
            }
        }

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            var originalMesh = expected.Meshes[i];
            var mesh = actual.Meshes[i];
            Equal(originalMesh, mesh);
        }

        for (int i = 0; i < expected.Nodes.Count; i++)
        {
            var originalNode = expected.Nodes[i];
            var node = actual.Nodes[i];

            Assert.Equal(originalNode.BoundingBoxMax, node.BoundingBoxMax);
            Assert.Equal(originalNode.BoundingBoxMin, node.BoundingBoxMin);
            Assert.Equal(originalNode.FirstChildIndex, node.FirstChildIndex);
            Assert.Equal(originalNode.Flags, node.Flags);
            Assert.Equal(originalNode.Name, node.Name);
            Assert.Equal(originalNode.NextSiblingIndex, node.NextSiblingIndex);
            Assert.Equal(originalNode.ParentIndex, node.ParentIndex);
            Assert.Equal(node.PreviousSiblingIndex, node.PreviousSiblingIndex);
            Assert.Equal(originalNode.Rotation, node.Rotation);
            Assert.Equal(originalNode.Scale, node.Scale);
            Assert.Equal(originalNode.Translation, node.Translation);
        }

        for (int i = 0; i < expected.Skeletons.AllSkeletons.Count; i++)
        {
            var originalNode = expected.Skeletons.AllSkeletons[i];
            var node = actual.Skeletons.AllSkeletons[i];

            Assert.Equal(originalNode.FirstChildIndex, node.FirstChildIndex);
            Assert.Equal(originalNode.NextSiblingIndex, node.NextSiblingIndex);
            Assert.Equal(originalNode.NodeIndex, node.NodeIndex);
            Assert.Equal(originalNode.ParentIndex, node.ParentIndex);
            Assert.Equal(originalNode.PreviousSiblingIndex, node.PreviousSiblingIndex);
        }

        for (int i = 0; i < expected.Skeletons.BaseSkeleton.Count; i++)
        {
            var originalNode = expected.Skeletons.AllSkeletons[i];
            var node = actual.Skeletons.AllSkeletons[i];

            Assert.Equal(originalNode.FirstChildIndex, node.FirstChildIndex);
            Assert.Equal(originalNode.NextSiblingIndex, node.NextSiblingIndex);
            Assert.Equal(originalNode.NodeIndex, node.NodeIndex);
            Assert.Equal(originalNode.ParentIndex, node.ParentIndex);
            Assert.Equal(originalNode.PreviousSiblingIndex, node.PreviousSiblingIndex);
        }


    }


    public static void EqualNoMaterialCheck(FLVER2.Mesh originalMesh, FLVER2.Mesh mesh)
    {
        Assert.Equal(originalMesh.BoneIndices, mesh.BoneIndices);
        if (originalMesh.BoundingBox != null)
        {
            Assert.Equal(originalMesh.BoundingBox.Max, mesh.BoundingBox.Max);
            Assert.Equal(originalMesh.BoundingBox.Min, mesh.BoundingBox.Min);
            Assert.Equal(originalMesh.BoundingBox.Unk, mesh.BoundingBox.Unk);
        }
        else
        {
            Assert.Equal(originalMesh.BoundingBox, mesh.BoundingBox);
        }

        Assert.Equal(originalMesh.Dynamic, mesh.Dynamic);
        Assert.Equal(originalMesh.NodeIndex, mesh.NodeIndex);
        Assert.Equal(originalMesh.UseBoneWeights, mesh.UseBoneWeights);

        Assert.Equal(originalMesh.VertexBuffers.Count, mesh.VertexBuffers.Count);
        for (int j = 0; j < originalMesh.VertexBuffers.Count; j++)
        {
            var originalVertexBuffers = originalMesh.VertexBuffers[j];
            var vertexBuffers = originalMesh.VertexBuffers[j];

            Assert.Equal(originalVertexBuffers.LayoutIndex, vertexBuffers.LayoutIndex);
        }

        Assert.Equal(originalMesh.Vertices.Count, mesh.Vertices.Count);
        for (int j = 0; j < originalMesh.Vertices.Count; j++)
        {
            var originalVertices = originalMesh.Vertices[j];
            var vertices = originalMesh.Vertices[j];

            Assert.Equal(originalVertices.Bitangent, vertices.Bitangent);
            Assert.Equal(originalVertices.BoneIndices, vertices.BoneIndices);
            Assert.Equal(originalVertices.BoneWeights, vertices.BoneWeights);
            Assert.Equal(originalVertices.Colors, vertices.Colors);
            Assert.Equal(originalVertices.Normal, vertices.Normal);
            Assert.Equal(originalVertices.NormalW, vertices.NormalW);
            Assert.Equal(originalVertices.Position.X, vertices.Position.X, 0.001);
            Assert.Equal(originalVertices.Position.Y, vertices.Position.Y, 0.001);
            Assert.Equal(originalVertices.Position.Z, vertices.Position.Z, 0.001);
            Assert.Equal(originalVertices.Tangents, vertices.Tangents);
            Assert.Equal(originalVertices.UVs, vertices.UVs);
        }

        Assert.Equal(originalMesh.FaceSets.Count, mesh.FaceSets.Count);

        for (int j = 0; j < originalMesh.FaceSets.Count; j++)
        {
            var originalVertices = originalMesh.FaceSets[j];
            var vertices = originalMesh.FaceSets[j];

            Assert.Equal(originalVertices.CullBackfaces, vertices.CullBackfaces);
            Assert.Equal(originalVertices.Flags, vertices.Flags);
            Assert.Equal(originalVertices.Indices, vertices.Indices);
            Assert.Equal(originalVertices.TriangleStrip, vertices.TriangleStrip);
            Assert.Equal(originalVertices.Unk06, vertices.Unk06);
        }
    }


    public static void Equal(FLVER2.Mesh originalMesh, FLVER2.Mesh mesh)
    {
        Assert.Equal(originalMesh.BoneIndices, mesh.BoneIndices);
        if (originalMesh.BoundingBox != null)
        {
            Assert.Equal(originalMesh.BoundingBox.Max, mesh.BoundingBox.Max);
            Assert.Equal(originalMesh.BoundingBox.Min, mesh.BoundingBox.Min);
            Assert.Equal(originalMesh.BoundingBox.Unk, mesh.BoundingBox.Unk);
        }
        else
        {
            Assert.Equal(originalMesh.BoundingBox, mesh.BoundingBox);
        }

        Assert.Equal(originalMesh.Dynamic, mesh.Dynamic);
        Assert.Equal(originalMesh.MaterialIndex, mesh.MaterialIndex);
        Assert.Equal(originalMesh.NodeIndex, mesh.NodeIndex);
        Assert.Equal(originalMesh.UseBoneWeights, mesh.UseBoneWeights);

        Assert.Equal(originalMesh.VertexBuffers.Count, mesh.VertexBuffers.Count);
        for (int j = 0; j < originalMesh.VertexBuffers.Count; j++)
        {
            var originalVertexBuffers = originalMesh.VertexBuffers[j];
            var vertexBuffers = originalMesh.VertexBuffers[j];

            Assert.Equal(originalVertexBuffers.LayoutIndex, vertexBuffers.LayoutIndex);
        }

        Assert.Equal(originalMesh.Vertices.Count, mesh.Vertices.Count);
        for (int j = 0; j < originalMesh.Vertices.Count; j++)
        {
            var originalVertices = originalMesh.Vertices[j];
            var vertices = originalMesh.Vertices[j];

            Assert.Equal(originalVertices.Bitangent, vertices.Bitangent);
            Assert.Equal(originalVertices.BoneIndices, vertices.BoneIndices);
            Assert.Equal(originalVertices.BoneWeights, vertices.BoneWeights);
            Assert.Equal(originalVertices.Colors, vertices.Colors);
            Assert.Equal(originalVertices.Normal, vertices.Normal);
            Assert.Equal(originalVertices.NormalW, vertices.NormalW);
            Assert.Equal(originalVertices.Position.X, vertices.Position.X, 0.001);
            Assert.Equal(originalVertices.Position.Y, vertices.Position.Y, 0.001);
            Assert.Equal(originalVertices.Position.Z, vertices.Position.Z, 0.001);

            Assert.Equal(originalVertices.Tangents, vertices.Tangents);
            Assert.Equal(originalVertices.UVs, vertices.UVs);
        }

        Assert.Equal(originalMesh.FaceSets.Count, mesh.FaceSets.Count);

        for (int j = 0; j < originalMesh.FaceSets.Count; j++)
        {
            var originalVertices = originalMesh.FaceSets[j];
            var vertices = originalMesh.FaceSets[j];

            Assert.Equal(originalVertices.CullBackfaces, vertices.CullBackfaces);
            Assert.Equal(originalVertices.Flags, vertices.Flags);
            Assert.Equal(originalVertices.Indices, vertices.Indices);
            Assert.Equal(originalVertices.TriangleStrip, vertices.TriangleStrip);
            Assert.Equal(originalVertices.Unk06, vertices.Unk06);
        }
    }

    public static void Equal(FLVER0 expected, FLVER0 actual)
    {
        Assert.Equal(expected.Compression, actual.Compression);
        Assert.Equal(expected.Header.BigEndian, actual.Header.BigEndian);
        Assert.Equal(expected.Header.BoundingBoxMax, actual.Header.BoundingBoxMax);
        Assert.Equal(expected.Header.BoundingBoxMin, actual.Header.BoundingBoxMin);
        Assert.Equal(expected.Header.Unicode, actual.Header.Unicode);
        Assert.Equal(expected.Header.Unk4A, actual.Header.Unk4A);
        Assert.Equal(expected.Header.Unk4B, actual.Header.Unk4B);
        Assert.Equal(expected.Header.Unk4C, actual.Header.Unk4C);
        Assert.Equal(expected.Header.Unk5C, actual.Header.Unk5C);
        Assert.Equal(expected.Header.VertexIndexSize, actual.Header.VertexIndexSize);
        Assert.Equal(expected.Header.Version, actual.Header.Version);


        Assert.Equal(expected.Dummies.Count, actual.Dummies.Count);
        Assert.Equal(expected.Materials.Count, actual.Materials.Count);
        Assert.Equal(expected.Meshes.Count, actual.Meshes.Count);
        Assert.Equal(expected.Nodes.Count, actual.Nodes.Count);

        for (int i = 0; i < expected.Dummies.Count; i++)
        {
            var originalDummy = expected.Dummies[i];
            var dummy = actual.Dummies[i];

            Assert.Equal(originalDummy.Position.X, dummy.Position.X, 0.001);
            Assert.Equal(originalDummy.Position.Y, dummy.Position.Y, 0.001);
            Assert.Equal(originalDummy.Position.Z, dummy.Position.Z, 0.001);
            Assert.Equal(originalDummy.Forward, dummy.Forward);
            Assert.Equal(originalDummy.Upward, dummy.Upward);
            Assert.Equal(originalDummy.ReferenceID, dummy.ReferenceID);
            Assert.Equal(originalDummy.ParentBoneIndex, dummy.ParentBoneIndex);
            Assert.Equal(originalDummy.AttachBoneIndex, dummy.AttachBoneIndex);
            Assert.Equal(originalDummy.Color, dummy.Color);
            Assert.Equal(originalDummy.Flag1, dummy.Flag1);
            Assert.Equal(originalDummy.UseUpwardVector, dummy.UseUpwardVector);
            Assert.Equal(originalDummy.Unk30, dummy.Unk30);
            Assert.Equal(originalDummy.Unk34, dummy.Unk34);
            Assert.Equal(originalDummy.Unk34, dummy.Unk34);
        }

        for (int i = 0; i < expected.Materials.Count; i++)
        {
            var originalMaterial = expected.Materials[i];
            var material = actual.Materials[i];

            Assert.Equal(originalMaterial.MTD, material.MTD);
            Assert.Equal(originalMaterial.Name, material.Name);

            Assert.Equal(originalMaterial.Layouts.Count, material.Layouts.Count);
            for (int j = 0; j < originalMaterial.Layouts.Count; j++)
            {
                var originalLayout = originalMaterial.Layouts[j];
                var layouts = material.Layouts[j];
                Assert.Equal(originalLayout.Count, layouts.Count);

                for (int k = 0; k < originalLayout.Count; k++)
                {
                    var originalBuffer = originalLayout[k];
                    var buffer = layouts[k];

                    Assert.Equal(originalBuffer.Index, buffer.Index);
                    Assert.Equal(originalBuffer.Semantic, buffer.Semantic);
                    Assert.Equal(originalBuffer.Size, buffer.Size);
                    Assert.Equal(originalBuffer.Unk00, buffer.Unk00);
                    Assert.Equal(originalBuffer.Type, buffer.Type);
                }
            }

            Assert.Equal(originalMaterial.Textures.Count, material.Textures.Count);
            for (int j = 0; j < originalMaterial.Textures.Count; j++)
            {
                var originalTexture = originalMaterial.Textures[j];
                var texture = originalMaterial.Textures[j];

                Assert.Equal(originalTexture.Path, texture.Path);
                Assert.Equal(originalTexture.Type, texture.Type);
            }
        }

        for (int i = 0; i < expected.Meshes.Count; i++)
        {
            var originalMesh = expected.Meshes[i];
            var mesh = actual.Meshes[i];

            Assert.Equal(originalMesh.BackfaceCulling, mesh.BackfaceCulling);
            Assert.Equal(originalMesh.BoneIndices, mesh.BoneIndices);
            Assert.Equal(originalMesh.Dynamic, mesh.Dynamic);
            Assert.Equal(originalMesh.LayoutIndex, mesh.LayoutIndex);
            Assert.Equal(originalMesh.MaterialIndex, mesh.MaterialIndex);
            Assert.Equal(originalMesh.UseTristrips, mesh.UseTristrips);
            Assert.Equal(originalMesh.Unk46, mesh.Unk46);
            Assert.Equal(originalMesh.DefaultBoneIndex, mesh.DefaultBoneIndex);
            Assert.Equal(originalMesh.VertexIndices, mesh.VertexIndices);

            Assert.Equal(originalMesh.Vertices.Count, mesh.Vertices.Count);
            for (int j = 0; j < originalMesh.Vertices.Count; j++)
            {
                var originalVertices = originalMesh.Vertices[j];
                var vertices = originalMesh.Vertices[j];

                Assert.Equal(originalVertices.Bitangent, vertices.Bitangent);
                Assert.Equal(originalVertices.BoneIndices, vertices.BoneIndices);
                Assert.Equal(originalVertices.BoneWeights, vertices.BoneWeights);
                Assert.Equal(originalVertices.Colors, vertices.Colors);
                Assert.Equal(originalVertices.Normal, vertices.Normal);
                Assert.Equal(originalVertices.NormalW, vertices.NormalW);
                Assert.Equal(originalVertices.Position.X, vertices.Position.X, 0.001);
                Assert.Equal(originalVertices.Position.Y, vertices.Position.Y, 0.001);
                Assert.Equal(originalVertices.Position.Z, vertices.Position.Z, 0.001);
                Assert.Equal(originalVertices.Tangents, vertices.Tangents);
                Assert.Equal(originalVertices.UVs, vertices.UVs);
            }
        }

        for (int i = 0; i < expected.Nodes.Count; i++)
        {
            var originalNode = expected.Nodes[i];
            var node = actual.Nodes[i];

            Assert.Equal(originalNode.BoundingBoxMax, node.BoundingBoxMax);
            Assert.Equal(originalNode.BoundingBoxMin, node.BoundingBoxMin);
            Assert.Equal(originalNode.FirstChildIndex, node.FirstChildIndex);
            Assert.Equal(originalNode.Flags, node.Flags);
            Assert.Equal(originalNode.Name, node.Name);
            Assert.Equal(originalNode.NextSiblingIndex, node.NextSiblingIndex);
            Assert.Equal(originalNode.ParentIndex, node.ParentIndex);
            Assert.Equal(node.PreviousSiblingIndex, node.PreviousSiblingIndex);
            Assert.Equal(originalNode.Rotation, node.Rotation);
            Assert.Equal(originalNode.Scale, node.Scale);
            Assert.Equal(originalNode.Translation, node.Translation);
        }

    }

}