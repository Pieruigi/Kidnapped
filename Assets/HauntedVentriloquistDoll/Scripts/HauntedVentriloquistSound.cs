/// <summary>
/// 
/// Haunted Ventriloquist Doll
/// 
/// This is a simple script JUST for showcase purposes (test scene). It coordinates some sounds based on variable iterations and pre-made transitions (animController demo).
/// 
/// NOTE> I do not give support for this script. Feel free to tweak and use it as a base for your own sounds/transitions.
/// 
/// 
/// </summary>

using UnityEngine;
using System.Collections;

public class HauntedVentriloquistSound : MonoBehaviour {
	
	[SerializeField]AudioClip[] sounds;

	private AudioSource _audioSource;
	private int iterations = 0;


	void Awake(){

		_audioSource = GetComponent <AudioSource> ();

	
	}

	// Use this for initialization
	void Start () {

		StartCoroutine (_mecanimSound());

	}
	

	private IEnumerator _mecanimSound(){

		Animator thisAnim = GetComponent<Animator> ();

		//anim hashes
		int creepiNess = Animator.StringToHash("Creepiness#1 PART 1");
		int creepiness2 = Animator.StringToHash ("Creepiness2");
        int sittingToStanding = Animator.StringToHash("STAND UP");
        int totalHaunted = Animator.StringToHash("Creepiness#3");

        while (true) {


			switch (iterations){

			case (int)iterationsName.creepiness : 

				//First sound to be applied
				if (thisAnim.GetCurrentAnimatorStateInfo (0).shortNameHash == creepiNess && !GetComponent<AudioSource>().isPlaying) {

                    yield return new WaitForSeconds(.4f);

					_audioSource.clip = sounds [(int)iterationsName.creepiness];
                   // _audioSource.pitch = 0.8f;
					GetComponent<AudioSource> ().Play ();

					
					yield return StartCoroutine (__nextIteration());

				}

				break;

			case (int)iterationsName.creepiness2 : 

				//Second sound to be applied
				yield return new WaitForSeconds (3.6f);

				if (thisAnim.GetCurrentAnimatorStateInfo(0).shortNameHash == creepiness2 && !GetComponent<AudioSource>().isPlaying){

					_audioSource.clip = sounds [(int)iterationsName.creepiness2];
					GetComponent<AudioSource> ().Play ();

					

					yield return StartCoroutine (__nextIteration());

				}
					
				break;

			case (int)iterationsName.standup:
				
				//Third
				if (thisAnim.GetNextAnimatorStateInfo(0).shortNameHash == sittingToStanding && !GetComponent<AudioSource>().isPlaying){

					_audioSource.clip = sounds [(int)iterationsName.standup];
			
					GetComponent<AudioSource> ().Play ();

                    yield return StartCoroutine(__nextIteration());

                }
				break;

                case (int)iterationsName.totalHaunted:

                    //Fourth
                    if (thisAnim.GetNextAnimatorStateInfo(0).shortNameHash == totalHaunted && !GetComponent<AudioSource>().isPlaying)
                    {

                        _audioSource.clip = sounds[(int)iterationsName.totalHaunted];

                        yield return new WaitForSeconds(.5f);

                        GetComponent<AudioSource>().Play();

               
                        yield return StartCoroutine(__nextIteration());
                    }
                    break;


            }


            if (iterations == 4){
				
				iterations = 0;
		

			}


            yield return null;
		}

	}



	private IEnumerator __nextIteration(){

		++iterations;
		yield return null;

	}
		

	private enum iterationsName{

		creepiness, creepiness2, standup, totalHaunted
	}
		

	
}
