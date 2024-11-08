using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TronPostProcessing : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public Material material;
        RenderTargetIdentifier source;
        RenderTargetHandle temporaryTexture;

        public CustomRenderPass(Material material)
        {
            this.material = material;
            temporaryTexture.Init("_TemporaryColorTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(temporaryTexture.id, cameraTextureDescriptor);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("TronPostProcessing");
            Blit(cmd, source, temporaryTexture.Identifier(), material);
            Blit(cmd, temporaryTexture.Identifier(), source);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(temporaryTexture.id);
        }
    }

    [System.Serializable]
    public class TronSettings
    {
        public Material material = null;
    }

    public TronSettings settings = new TronSettings();
    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        if (settings.material != null)
        {
            m_ScriptablePass = new CustomRenderPass(settings.material);
            m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (m_ScriptablePass != null && settings.material != null)
        {
            m_ScriptablePass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
}
