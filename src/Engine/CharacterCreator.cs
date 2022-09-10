using Godot;

public class CharacterCreator : Node2D
{
	private GameData gameData;
	private CanvasItem nameEntry;
	private PlayerModel playerModel;
	private PlayerIcon playerIcon;

	public override void _Ready()
	{
		GetNode<AudioEngine>("/root/AudioEngine").FadeMusic("Music/SometimesWeGetToSeeTheSunRise_Calm.ogg");
		gameData = Global.data;
		playerModel = GetNode<PlayerModel>("PlayerModel");
		nameEntry = GetNode<CanvasItem>("UI/NameEntry");
		playerIcon = GetNode<PlayerIcon>("UI/NameEntry/PlayerIcon");

		nameEntry.Hide();
		playerModel.UpdateModel();
	}

	public void Randomize()
	{
		gameData.avaData.Randomize();
		foreach (Node i in GetNode("UI/Windows").GetChildren())
		{
			if (i.Name == "Colors") continue;
			
			Node margin = i.GetNode<MarginContainer>("ScrollContainer/MarginContainer");
			int randomizedEnum = 0;

			if (i.Name == "Body") randomizedEnum = (int) gameData.avaData.body;
			else if (i.Name == "Head") randomizedEnum = (int) gameData.avaData.head;
			else if (i.Name == "Brow") randomizedEnum = (int) gameData.avaData.brow;
			else if (i.Name == "Eye") randomizedEnum = (int) gameData.avaData.eye;
			else if (i.Name == "Nose") randomizedEnum = (int) gameData.avaData.nose;
			else if (i.Name == "Hair") randomizedEnum = (int) gameData.avaData.hair;
			else if (i.Name == "Sideburn") randomizedEnum = (int) gameData.avaData.sideburn;
			else if (i.Name == "Moustache") randomizedEnum = (int) gameData.avaData.moustache;
			else if (i.Name == "Beard") randomizedEnum = (int) gameData.avaData.beard;
			else if (i.Name == "Scruff") randomizedEnum = (int) gameData.avaData.scruff;

			BaseButton randomizedButton = margin.GetNode("Grid").GetChild<BaseButton>(randomizedEnum);
			margin.GetNode<Selector>("Selector").Select(randomizedButton);
		}


		playerModel.UpdateModel();
	}

	public void ChangeOption(PlayerEnum type, int index)
	{
		if (type == PlayerEnum.Body) gameData.avaData.body = (PlayerBody) index;
		else if (type == PlayerEnum.Head) gameData.avaData.head = (PlayerHead) index;
		else if (type == PlayerEnum.Eye) gameData.avaData.eye = (PlayerEye) index;
		else if (type == PlayerEnum.Nose) gameData.avaData.nose = (PlayerNose) index;
		else if (type == PlayerEnum.Brow) gameData.avaData.brow = (PlayerBrow) index;
		else if (type == PlayerEnum.Hair) gameData.avaData.hair = (PlayerHair) index;
		else if (type == PlayerEnum.Sideburn) gameData.avaData.sideburn = (PlayerSideburn) index;
		else if (type == PlayerEnum.Beard) gameData.avaData.beard = (PlayerBeard) index;
		else if (type == PlayerEnum.Moustache) gameData.avaData.moustache = (PlayerMoustache) index;
		else if (type == PlayerEnum.Scruff) gameData.avaData.scruff = (PlayerScruff) index;

		else if (type == PlayerEnum.Skin) gameData.avaData.skin = (PlayerSkin) index;
		else if (type == PlayerEnum.HairColor) gameData.avaData.hairColor = (PlayerColor) index;
		else if (type == PlayerEnum.MoustacheColor) gameData.avaData.moustacheColor = (PlayerColor) index;
		else if (type == PlayerEnum.BeardColor)
		{
			gameData.avaData.beardColor = (PlayerColor) index;
			gameData.avaData.moustacheColor = (PlayerColor) index;
		}
		else if (type == PlayerEnum.BrowColor) gameData.avaData.browColor = (PlayerColor) index;
		else if (type == PlayerEnum.EyeColor) gameData.avaData.eyeColor = (PlayerEyeColor) index;

		else if (type == PlayerEnum.AllHairColor)
		{
			gameData.avaData.hairColor = (PlayerColor) index;
			gameData.avaData.browColor = (PlayerColor) index;
			gameData.avaData.beardColor = (PlayerColor) index;
			gameData.avaData.moustacheColor = (PlayerColor) index;
		}

		playerModel.UpdateModel();
	}

	public void NameEntry()
	{
		playerIcon.UpdateIcon();
		LineEdit line = nameEntry.GetNode<LineEdit>("LineEdit");
		line.CaretPosition = line.Text.Length;
		line.GrabFocus();

		nameEntry.GetNode<AnimationPlayer>("AnimationPlayer").Stop();
		nameEntry.GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeIn");
	}

	public void NameEntryBack()
	{
		nameEntry.GetNode<AnimationPlayer>("AnimationPlayer").Play("FadeOut");
	}

	public void Finish()
	{
		gameData.avaData.name = nameEntry.GetNode<LineEdit>("LineEdit").Text;
		
	}
}
