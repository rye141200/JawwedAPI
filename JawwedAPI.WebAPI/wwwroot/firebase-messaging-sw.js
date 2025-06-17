// 1) Import compat libraries
importScripts(
  "https://www.gstatic.com/firebasejs/11.6.1/firebase-app-compat.js"
);
importScripts(
  "https://www.gstatic.com/firebasejs/11.6.1/firebase-messaging-compat.js"
);

// 2) Your config (must match the page)
const firebaseConfig = {
  apiKey: "AIzaSyB7YzM_OQ0Uq_aEyz5Yn3rgQ6NWdz7lxfU",
  authDomain: "jawwedapi-2cfdf.firebaseapp.com",
  projectId: "jawwedapi-2cfdf",
  storageBucket: "jawwedapi-2cfdf.firebasestorage.app",
  messagingSenderId: "151202375322",
  appId: "1:151202375322:web:6586a77d24525a0ffce123",
  measurementId: "G-YP691M6Q0W",
};

// 3) Initialize compat app & get messaging
firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();

// 4) Handle background messages
messaging.onBackgroundMessage((payload) => {
  console.log("[SW] Background message received:", payload);
  const { title, body } = payload.notification || {};
  self.registration.showNotification(title, { body });
});

// 5) Handle push subscription change
self.addEventListener("pushsubscriptionchange", (event) => {
  event.waitUntil(
    self.clients
      .matchAll({ type: "window" })
      .then((clients) => clients[0]?.postMessage("REFRESH_FCM_TOKEN"))
  );
});
