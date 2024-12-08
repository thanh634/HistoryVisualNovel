using CHARACTER;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class Testing_Audio : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Running());
        }
        Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
        // Update is called once per frame
        IEnumerator Running()
        {
            Character_Sprite linh1 = CreateCharacter("linh 1 as Linh Dai Viet") as Character_Sprite;
            Character_Sprite linh2 = CreateCharacter("linh 2 as Linh Dai Viet") as Character_Sprite;


            linh1.Show();

            yield return new WaitForSeconds(1f);

            GraphicPanelManager.instance.GetPanel("background").GetLayer(0, true).SetTexture("Graphics/BG Images/5");
            AudioManager.instance.PlayTrack("Audio/Ambience/RainyMood", 0);
            AudioManager.instance.PlayTrack("Audio/Music/Calm", 1, pitch: 0.7f);
            //AudioManager.instance.PlayVoice("Audio/Voices/exclamation");

            yield return new WaitForSeconds(1f);


            AudioManager.instance.PlaySoundEffect("Audio/SFX/thunder_strong_01");

            yield return new WaitForSeconds(2f);
            linh1.Animate("Hop");
            yield return linh1.Say("Het hon");
            AudioManager.instance.PlayTrack("Audio/Music/Calm2", 1, pitch: 0.7f);

            yield return new WaitForSeconds(5f);

            AudioManager.instance.StopTrack(1);



        }

        IEnumerator Running2()
        {
            AudioChannel channel = new AudioChannel(1);

            yield return null;
        }
    }
}