using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTER;
using DIALOGUE;

namespace TESTING
{
    public class Testing_Character : MonoBehaviour
    {
<<<<<<< HEAD
        private VisualNovelCharater CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
=======
        private VNCharacter CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);
>>>>>>> mergeCombat

        // Start is called before the first frame update
        void Start()
        {
            //VisualNovelCharater vua = CharacterManager.instance.CreateCharacter("Tran Nhan Tong");
            //VisualNovelCharater tranhungdao = CharacterManager.instance.CreateCharacter("Tran Hung Dao");
            //VisualNovelCharater vuabunhin = CharacterManager.instance.CreateCharacter("Tran Nhan Tong");

            StartCoroutine(Test2());
        }

        IEnumerator Test2()
        {
            /*
            
            // Create a character using "Linh Dai Viet" sprite
            VisualNovelCharater linh1 = CharacterManager.instance.CreateCharacter("Linh Dai Viet");

            // Create a sprite character using "Linh Dai Viet" sprite, this one have name, and can do many other stuff below
            Character_Sprite linh1 = CreateCharacter("Linh1 as Linh Dai Viet") as  Character_Sprite;

            yield return linh1.Flip(); // Immediately flip direction
            yield return linh1.Flip(immediate: false); // Flip direction by fading
            yield return linh1.Flip(0.5f, false); // Flip direction by fading in with a speed

            // Move to a position, can take in speed, smooth make the animation better
            yield return linh1.MoveToPosition(new Vector2(0.5f, 0.5f), smooth : true); 

            // Set the the prefer sprite of a character
            Sprite linhSprite = linh1.GetSprite("1");
            linh1.TransitionSprite(linhSprite);
            linh1.TransitionSprite(linh1.GetSprite("2")); 

            linh1.SetColor(Color.red); // Change character's color

            yield return linh1.Highlight(); // Highlight a character
            yield return linh1.UnHighlight(); // Unhighlight a character

            yield return linh1.Say("May ga"); // Make a characer talk

            // set character's visibility
            yield return linh.Hide();
            yield return linh.Show();

            */
            Character_Sprite vua = CreateCharacter("Tran Nhan Tong") as Character_Sprite;
            Character_Sprite tuong = CreateCharacter("Tran Hung Dao") as Character_Sprite;

            Character_Sprite linh1 = CreateCharacter("Linh1 as Linh Dai Viet") as  Character_Sprite;
            Character_Sprite linh2 = CreateCharacter("Linh2 as Linh Nguyen Mong") as Character_Sprite;
            Character_Sprite linh3 = CreateCharacter("Linh3 as Linh Dai Viet") as Character_Sprite;
            Character_Sprite linh4 = CreateCharacter("Linh4 as Linh Nguyen Mong") as Character_Sprite;

            linh3.TransitionSprite(linh3.GetSprite("1"));
            linh4.TransitionSprite(linh4.GetSprite("2"));

            vua.SetPosition(new Vector2(0.3f, 0));
            tuong.SetPosition(new Vector2(0.45f, 0));
            linh1.SetPosition(new Vector2(0.6f, 0));
            linh2.SetPosition(new Vector2(0.75f, 0));
            //linh3.SetPosition(new Vector2(0.2f, 0));
            //linh4.SetPosition(new Vector2(0.3f, 0));

            vua.Show();
            tuong.Show();
            linh1.Show();
            linh2.Show();
            //linh3.Show();
            //linh4.Show();

            linh2.SetPriority(1000);
            tuong.SetPriority(15);
            vua.SetPriority(8);
            linh1.SetPriority(30);

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters(new string[] {"Tran Hung Dao", "Tran Nhan Tong"});
            vua.Animate("Hop");

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters();

            yield return new WaitForSeconds(1);

            CharacterManager.instance.SortCharacters(new string[] { "Tran Nhan Tong", "Linh2", "Linh1", "Tran Hung Dao" });
            linh1.Animate("Hop");
            linh2.Animate("Shiver", true);

            yield return new WaitForSeconds(1);

            linh2.Animate("Shiver", false);


            yield return null;

        }

        IEnumerator Test()
        {
            yield return new WaitForSeconds(1);

<<<<<<< HEAD
            VisualNovelCharater linh = CharacterManager.instance.CreateCharacter("Linh Dai Viet");
=======
            VNCharacter linh = CharacterManager.instance.CreateCharacter("Linh Dai Viet");
>>>>>>> mergeCombat

            yield return new WaitForSeconds(1);

            yield return linh.Hide();

            yield return new WaitForSeconds(1);

            yield return linh.Show();

            yield return linh.Say("Tu noi dong xanh thom huong lua");


            Debug.Log("Finished");

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}