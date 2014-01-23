
private var timer : float = 0;
private var haveClicked : boolean = false;
private var radius : float;

function Start () {
	radius = 0.5*Mathf.Min(Screen.width,Screen.height);
	guiTexture.color.a = 0;
	Screen.showCursor = false;
	Screen.lockCursor = true;
	guiTexture.pixelInset = Rect (-radius, -radius, 2*radius, 2*radius);
}

function Update () {

	timer += Time.deltaTime;
	if ((Input.anyKeyDown)&&(!haveClicked)) {
		if (timer<2) timer = 6-timer;
		else if (timer<4) timer = 4;
		haveClicked = true;
	}
	if (timer<2) guiTexture.color.a += Time.deltaTime/2;
	else if (timer>4) guiTexture.color.a -= Time.deltaTime/2;
	if (timer>6) Application.LoadLevel(1);

}
