// Define el nombre de la caché
var CACHE_NAME = 'my-pwa-cache';

// Lista de archivos que deseas almacenar en caché
var urlsToCache = [
    '/Content/CssBot.css',
    '/Imagenes/icono.ico',

];



// Instalación del Service Worker
self.addEventListener('install', function (event) {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(function (cache) {
                return cache.addAll(urlsToCache);
            })
    );
});


// Activación del Service Worker
self.addEventListener('activate', function (event) {
    // Elimina cachés antiguas cuando el Service Worker es activado
    event.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames.filter(function (cacheName) {
                    return cacheName !== CACHE_NAME;
                }).map(function (cacheName) {
                    return caches.delete(cacheName);
                })
            );
        })
    );
});

// Interceptar solicitudes y responder con recursos almacenados en caché
self.addEventListener('fetch', function (event) {
    event.respondWith(
        caches.match(event.request)
            .then(function (response) {
                // Cache hit - return response
                if (response) {
                    return response;
                }
                // No cache match - pass through to network
                return fetch(event.request);
            })
    );
});

