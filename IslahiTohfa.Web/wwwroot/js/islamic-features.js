// Islamic Features JavaScript for Islahi Tohfa
(function () {
    'use strict';

    // Islamic Calendar and Prayer Times
    class IslamicFeatures {
        constructor() {
            this.init();
        }

        init() {
            this.setupPrayerTimeIndicator();
            this.setupIslamicCalendar();
            this.setupReadingProgress();
            this.setupKeyboardShortcuts();
            this.setupServiceWorker();
        }

        // Prayer Time Indicator
        setupPrayerTimeIndicator() {
            const indicator = document.createElement('div');
            indicator.className = 'prayer-time-indicator';
            indicator.style.display = 'none';

            // Get user's location and calculate prayer times
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    (position) => {
                        this.calculatePrayerTimes(position.coords.latitude, position.coords.longitude);
                    },
                    () => {
                        // Default to Riyadh coordinates
                        this.calculatePrayerTimes(24.7136, 46.6753);
                    }
                );
            }
        }

        calculatePrayerTimes(lat, lng) {
            // Simple prayer time calculation (use a proper library like adhan.js in production)
            const now = new Date();
            const prayerTimes = {
                fajr: '05:30',
                dhuhr: '12:15',
                asr: '15:45',
                maghrib: '18:20',
                isha: '19:50'
            };

            this.displayNextPrayer(prayerTimes);
        }

        displayNextPrayer(times) {
            const now = new Date();
            const currentTime = now.getHours() * 60 + now.getMinutes();

            const prayers = [
                { name: 'الفجر', time: times.fajr, nameEn: 'Fajr' },
                { name: 'الظهر', time: times.dhuhr, nameEn: 'Dhuhr' },
                { name: 'العصر', time: times.asr, nameEn: 'Asr' },
                { name: 'المغرب', time: times.maghrib, nameEn: 'Maghrib' },
                { name: 'العشاء', time: times.isha, nameEn: 'Isha' }
            ];

            let nextPrayer = prayers[0]; // Default to Fajr

            for (let prayer of prayers) {
                const [hours, minutes] = prayer.time.split(':');
                const prayerTime = parseInt(hours) * 60 + parseInt(minutes);

                if (prayerTime > currentTime) {
                    nextPrayer = prayer;
                    break;
                }
            }

            const indicator = document.querySelector('.prayer-time-indicator');
            if (indicator) {
                indicator.innerHTML = `
                    <i class="fas fa-mosque"></i>
                    ${nextPrayer.name} ${nextPrayer.time}
                `;
                indicator.style.display = 'block';
                document.body.appendChild(indicator);
            }
        }

        // Islamic Calendar
        setupIslamicCalendar() {
            const calendarElement = document.querySelector('.islamic-calendar');
            if (calendarElement) {
                const hijriDate = this.getHijriDate();
                const gregorianDate = new Date().toLocaleDateString('ar-SA');

                calendarElement.innerHTML = `
                    <div class="hijri-date">${hijriDate}</div>
                    <div class="gregorian-date">${gregorianDate}</div>
                `;
            }
        }

        getHijriDate() {
            // Simple Hijri date calculation (use a proper library like moment-hijri in production)
            const gregorianDate = new Date();
            const hijriYear = gregorianDate.getFullYear() - 579;
            const hijriMonth = gregorianDate.getMonth() + 1;
            const hijriDay = gregorianDate.getDate();

            const monthNames = [
                'محرم', 'صفر', 'ربيع الأول', 'ربيع الآخر', 'جمادى الأولى', 'جمادى الآخرة',
                'رجب', 'شعبان', 'رمضان', 'شوال', 'ذو القعدة', 'ذو الحجة'
            ];

            return `${hijriDay} ${monthNames[hijriMonth - 1]} ${hijriYear}هـ`;
        }

        // Reading Progress Tracker
        setupReadingProgress() {
            let readingStartTime = null;
            let currentPage = 1;

            // Track when user starts reading
            document.addEventListener('visibilitychange', () => {
                if (document.hidden) {
                    this.saveReadingProgress();
                } else {
                    readingStartTime = new Date();
                }
            });

            // Save progress when user leaves
            window.addEventListener('beforeunload', () => {
                this.saveReadingProgress();
            });
        }

        saveReadingProgress() {
            const progressData = {
                bookId: this.getCurrentBookId(),
                page: this.getCurrentPage(),
                timestamp: new Date(),
                duration: this.getReadingDuration()
            };

            localStorage.setItem('reading_progress', JSON.stringify(progressData));

            // Send to server if user is logged in
            if (this.isUserLoggedIn()) {
                this.sendProgressToServer(progressData);
            }
        }

        getCurrentBookId() {
            const path = window.location.pathname;
            const match = path.match(/\/book\/(\d+)/);
            return match ? parseInt(match[1]) : null;
        }

        getCurrentPage() {
            // Get current page from PDF viewer or reading interface
            const pageElement = document.querySelector('[data-current-page]');
            return pageElement ? parseInt(pageElement.dataset.currentPage) : 1;
        }

        getReadingDuration() {
            const progress = localStorage.getItem('reading_progress');
            if (progress) {
                const data = JSON.parse(progress);
                const startTime = new Date(data.timestamp);
                return new Date() - startTime;
            }
            return 0;
        }

        isUserLoggedIn() {
            return localStorage.getItem('authToken') !== null;
        }

        sendProgressToServer(progressData) {
            fetch('/api/books/track-activity', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${localStorage.getItem('authToken')}`
                },
                body: JSON.stringify({
                    bookId: progressData.bookId,
                    activityType: 'ReadingProgress',
                    pageNumber: progressData.page,
                    readingDuration: progressData.duration,
                    additionalData: JSON.stringify(progressData)
                })
            }).catch(console.error);
        }

        // Keyboard Shortcuts
        setupKeyboardShortcuts() {
            document.addEventListener('keydown', (e) => {
                // Only apply shortcuts when not in input fields
                if (e.target.tagName === 'INPUT' || e.target.tagName === 'TEXTAREA') {
                    return;
                }

                switch (e.key) {
                    case '/':
                        e.preventDefault();
                        this.focusSearchBox();
                        break;
                    case 'b':
                        if (e.ctrlKey || e.metaKey) {
                            e.preventDefault();
                            this.toggleBookmarks();
                        }
                        break;
                    case 'h':
                        if (e.ctrlKey || e.metaKey) {
                            e.preventDefault();
                            window.location.href = '/';
                        }
                        break;
                    case 'Escape':
                        this.closeModals();
                        break;
                }
            });
        }

        focusSearchBox() {
            const searchBox = document.querySelector('input[placeholder*="بحث"], input[placeholder*="Search"]');
            if (searchBox) {
                searchBox.focus();
            }
        }

        toggleBookmarks() {
            const bookmarksButton = document.querySelector('[href="/bookmarks"]');
            if (bookmarksButton) {
                bookmarksButton.click();
            }
        }

        closeModals() {
            const closeButtons = document.querySelectorAll('.mud-dialog-close, .mud-overlay');
            if (closeButtons.length > 0) {
                closeButtons[0].click();
            }
        }

        // Service Worker for PWA
        setupServiceWorker() {
            if ('serviceWorker' in navigator) {
                window.addEventListener('load', () => {
                    navigator.serviceWorker.register('/sw.js')
                        .then((registration) => {
                            console.log('SW registered: ', registration);
                            this.setupUpdateNotification(registration);
                        })
                        .catch((registrationError) => {
                            console.log('SW registration failed: ', registrationError);
                        });
                });
            }
        }

        setupUpdateNotification(registration) {
            registration.addEventListener('updatefound', () => {
                const newWorker = registration.installing;
                newWorker.addEventListener('statechange', () => {
                    if (newWorker.state === 'installed' && navigator.serviceWorker.controller) {
                        this.showUpdateNotification();
                    }
                });
            });
        }

        showUpdateNotification() {
            const notification = document.createElement('div');
            notification.className = 'update-notification';
            notification.innerHTML = `
                <div style="background: var(--islamic-primary); color: white; padding: 1rem; position: fixed; top: 0; left: 0; right: 0; z-index: 10000; text-align: center;">
                    <span>تحديث جديد متاح / New update available</span>
                    <button onclick="window.location.reload()" style="margin-left: 1rem; background: white; color: var(--islamic-primary); border: none; padding: 0.5rem 1rem; border-radius: 4px; cursor: pointer;">
                        تحديث / Update
                    </button>
                    <button onclick="this.parentElement.parentElement.remove()" style="margin-left: 0.5rem; background: transparent; color: white; border: 1px solid white; padding: 0.5rem 1rem; border-radius: 4px; cursor: pointer;">
                        لاحقاً / Later
                    </button>
                </div>
            `;
            document.body.appendChild(notification);
        }
    }

    // Initialize Islamic Features when DOM is ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', () => {
            new IslamicFeatures();
        });
    } else {
        new IslamicFeatures();
    }

    // Utility Functions
    window.IslamicUtils = {
        // Convert numbers to Arabic numerals
        toArabicNumerals: function (str) {
            const arabicNumerals = ['٠', '١', '٢', '٣', '٤', '٥', '٦', '٧', '٨', '٩'];
            return str.replace(/\d/g, (digit) => arabicNumerals[parseInt(digit)]);
        },

        // Format Islamic date
        formatIslamicDate: function (date) {
            const options = {
                year: 'numeric',
                month: 'long',
                day: 'numeric',
                calendar: 'islamic'
            };
            return new Intl.DateTimeFormat('ar-SA-u-ca-islamic', options).format(date);
        },

        // Get Qibla direction (simplified)
        getQiblaDirection: function (lat, lng) {
            const meccaLat = 21.4225;
            const meccaLng = 39.8262;

            const dLng = (meccaLng - lng) * Math.PI / 180;
            const lat1 = lat * Math.PI / 180;
            const lat2 = meccaLat * Math.PI / 180;

            const y = Math.sin(dLng) * Math.cos(lat2);
            const x = Math.cos(lat1) * Math.sin(lat2) - Math.sin(lat1) * Math.cos(lat2) * Math.cos(dLng);

            let bearing = Math.atan2(y, x) * 180 / Math.PI;
            bearing = (bearing + 360) % 360;

            return bearing;
        },

        // Validate Arabic text
        isValidArabicText: function (text) {
            const arabicRegex = /^[\u0600-\u06FF\s\d.,!?()]+$/;
            return arabicRegex.test(text);
        }
    };

})();