declare var require: any;
const webpush = require('web-push');

const publicKeyOutput = document.getElementById('generatedPublicKey') as HTMLDivElement;
const privateKeyOutput = document.getElementById('generatedPrivateKey') as HTMLDivElement;
const generateKeysBtn = document.getElementById('generateKeysBtn') as HTMLButtonElement;

generateKeysBtn.addEventListener("click", onGenerateKeysBtnClick);

function onGenerateKeysBtnClick() {
	const vapidKeys = webpush.generateVAPIDKeys();

	publicKeyOutput.innerText = vapidKeys.publicKey;
	privateKeyOutput.innerText = vapidKeys.privateKey;
}