# SimWorld_TT

## The systems
As the technical proof is incomplete there is more to talk about the thought process rather than the systems themselves, as the systems are incomplete.
Anyway I will explain the initial flow and what the systems were intended for even though the systems are incomplete. Just before that, I will mention the third party packages that you should not pay too much attention to as I hardly used them.
### Third party assets
Each selected folder in the image is part of third party asset packs
- HappyHarvest: Unity example for a game like Strawdey Valley
- KrishnaPalacio: Sprites and animations pack for a pixel art dungeon game
- Photon: SDK for multiplayer games
- Settings: Settings folder used by HappyHarvest
- ThirdParty/SceneReference: SceneReference is a single class, a wrapper that provides the means to safely serialize Scene Asset References

![image](https://github.com/jLautaroCabral/SimWorld_TT/assets/58992244/9a52dd1d-0c4d-4c52-a268-356bdee0f547)

### Systems: Managers and Locator
As usual, there are "Managers" classes in the game code. So far the managers that exist in the game are:
- NavigationManager: Responsible for the navigation between scenes, transition animations and loading screen.
- CamaraManager: Access point to get the camera that is currently rendering the scene, also contains logic related to switching cameras between spectator cameras (default) and character tracking cameras.
- OverlayManager: Contains logic to start, show and hide "overlays", to summarize its logic we can think that it works as a UI Stack. Basically it serves to accumulate UI.
- DialogManager: Exactly the same as the OverlayManager, but instead of being a UI Stack it is a UI Queue. Basically it serves for UI queues, useful for dialogs that sometimes come one after the other.
The managers are initialized by the ManagersInitilizer class, and then they are registered in the Locator. The Locator is simply a dictionary of managers in memory, used to avoid abuse of the Singleton pattern in managers.

![image](https://github.com/jLautaroCabral/SimWorld_TT/assets/58992244/b851dcd2-39e2-42f8-8497-48c4e9cb9101)

### Systems: Others
Some scripts of the project finally did not have a very important use, and some had no use (but will have it soon). These are:
- EventBus: I'm a big fan of the EventBus, so I was planning to implement it, but so far it wasn't a priority.
- GameLoader: Simply loads the main menu when the game starts at scene 0.
- IDependencyInjectable: Currently used by managers for initialization, although it is part of good practices it is not very common to need it in videogames (at least in indie videogames).
- NetGame: When trying to make the online game it was one of the first classes that I created, it has commented code related to PhotonBR.

![image](https://github.com/jLautaroCabral/SimWorld_TT/assets/58992244/86aecd53-4f8e-48be-8106-51070000b19e)
