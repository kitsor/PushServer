import { StringUtils } from './utils/string-utils'
import { WpsManager, IWpsManagerOptions } from './wpsManager'

const infoMessage = document.getElementById('infoMessage') as HTMLElement;
const subscribeBtn = document.getElementById('subscribeBtn') as HTMLButtonElement;

infoMessage.innerText = 'Loading...';
subscribeBtn.innerText = 'Subscribe';
subscribeBtn.disabled = true;

const manager = new WpsManager(wpsOptions);

manager.init()
	.then(m => {
		if (!m.isSupported()) {
			infoMessage.innerText = 'This browser does not support Push Notifications';
			return;
		}

		if (m.isDisabled()) {
			infoMessage.innerText = 'Push Notifications are disabled. Please enable the Notifications and reload the page';
			return;
		}

		subscribeBtn.disabled = false;

		if (m.isSubscribed()) {
			onSubscribe();
		}	else {
			infoMessage.innerText = 'This browser supports Push Notifications!';
		}

		subscribeBtn.addEventListener("click", subscribeBtnClick);

	})


function subscribeBtnClick(e: MouseEvent) {
	if (!manager.isSubscribed()) {
		manager.subscribe()
			.then(x => {
				if (x) {
					onSubscribe();
					return;
				}
				infoMessage.innerText = 'Something went wrong!';
			})
			.catch(error => { infoMessage.innerText = error.message; });
	} else {
		manager.unsubscribe()
			.then(x => {
				if (x) {
					onUnsubscribe();
					return;
				}
				infoMessage.innerText = 'Something went wrong!';
			});
	}

}

function onSubscribe() {
	infoMessage.innerText = 'Subscribed!';
	subscribeBtn.innerText = 'Unsubscribe';
}

function onUnsubscribe() {
	infoMessage.innerText = 'Unsubscribed!';
	subscribeBtn.innerText = 'Subscribe';
}

// global variables
declare let wpsOptions: IWpsManagerOptions;
