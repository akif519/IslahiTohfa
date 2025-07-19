const CACHE_NAME = 'islahi-tohfa-v1.0.0';
const urlsToCache = [
    '/',
    '/css/app.css',
    '/css/islamic-theme.css',
    '/js/app.js',
    '/js/islamic-features.js',
    '/images/islahi-tohfa-logo.png',
    '/_content/MudBlazor/MudBlazor.min.css',
    '/_content/MudBlazor/MudBlazor.min.js',
    '/manifest.json'
];

// Install Service Worker
self.addEventListener('install', (event) => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then((cache) => {
                console.log('Opened cache');
                return cache.addAll(urlsToCache);
            })
    );
});

// Fetch Event
self.addEventListener('fetch', (event) => {
    event.respondWith(
        caches.match(event.request)
            .then((response) => {
                // Return cached version or fetch from network
                if (response) {
                    return response;
                }

                // Clone the request because it's a stream
                const fetchRequest = event.request.clone();

                return fetch(fetchRequest).then((response) => {
                    // Check if valid response
                    if (!response || response.status !== 200 || response.type !== 'basic') {
                        return response;
                    }

                    // Clone the response because it's a stream
                    const responseToCache = response.clone();

                    caches.open(CACHE_NAME)
                        .then((cache) => {
                            cache.put(event.request, responseToCache);
                        });

                    return response;
                });
            })
    );
});

// Activate Service Worker
self.addEventListener('activate', (event) => {
    event.waitUntil(
        caches.keys().then((cacheNames) => {
            return Promise.all(
                cacheNames.map((cacheName) => {
                    if (cacheName !== CACHE_NAME) {
                        console.log('Deleting old cache:', cacheName);
                        return caches.delete(cacheName);
                    }
                })
            );
        })
    );
});

// Background Sync for offline functionality
self.addEventListener('sync', (event) => {
    if (event.tag === 'reading-progress') {
        event.waitUntil(syncReadingProgress());
    }
});

function syncReadingProgress() {
    // Sync reading progress when online
    return new Promise((resolve) => {
        const progress = localStorage.getItem('reading_progress');
        if (progress) {
            fetch('/api/books/track-activity', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('authToken')}`
                },
                body: progress
            }).then(() => {
                resolve();
            }).catch(() => {
                resolve(); // Resolve anyway to prevent infinite retries
            });
        } else {
            resolve();
        }
    });
}

// Push Notification handling
self.addEventListener('push', (event) => {
    const data = event.data ? event.data.json() : {};
    const title = data.title || 'الإصلاحي تحفة';
    const options = {
        body: data.body || 'رسالة جديدة',
        icon: '/images/icons/icon-192x192.png',
        badge: '/images/icons/icon-72x72.png',
        vibrate: [200, 100, 200],
        data: data.url || '/',
        actions: [
            {
                action: 'open',
                title: 'فتح / Open'
            },
            {
                action: 'close',
                title: 'إغلاق / Close'
            }
        ]
    };

    event.waitUntil(
        self.registration.showNotification(title, options)
    );
});

// Notification click handling
self.addEventListener('notificationclick', (event) => {
    event.notification.close();

    if (event.action === 'open') {
        event.waitUntil(
            clients.openWindow(event.notification.data)
        );
    }
});