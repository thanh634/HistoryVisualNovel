using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using DIALOGUE;

namespace TESTING
{
    public class Testing_GraphicPanel : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());

            //layer.currentGraphic.renderer.material.SetColor("_Color", Color.red);
        }

        // Update is called once per frame
        IEnumerator Running()
        {
            GraphicPanel panel = GraphicPanelManager.instance.GetPanel("background");
            GraphicLayer layer0 = panel.GetLayer(0, true);
            GraphicLayer layer1 = panel.GetLayer(1, true);

            Texture blendTex = Resources.Load<Texture>("Graphics/Transition Effects/hurricane");

            layer0.SetVideo("Graphics/BG Videos/Nebula");
            layer1.SetTexture("Graphics/BG Images/Spaceshipinterior");

            yield return new WaitForSeconds(2);

            GraphicPanel cinematic = GraphicPanelManager.instance.GetPanel("cinematic");
            GraphicLayer cinLayer = cinematic.GetLayer(0, true);

            Character vua = CharacterManager.instance.CreateCharacter("Tran Nhan Tong", true);

            yield return vua.Say("Let's take a look at the picture on cinematic layer.");

            cinLayer.SetTexture("Graphics/Gallery/pup");

            yield return DialogueSystem.instance.Say("Narrator", "We truly don't deserve dogs");

            cinLayer.Clear();

            yield return new WaitForSeconds(2);

            panel.Clear();

            //layer.SetTexture("Graphics/BG Images/2", blendingTexture: blendTex);

            //layer.SetVideo("Graphics/BG Videos/Fantasy Landscape", blendingTexture : blendTex);

            //layer.currentGraphic.FadeOut();
        }
    }
}