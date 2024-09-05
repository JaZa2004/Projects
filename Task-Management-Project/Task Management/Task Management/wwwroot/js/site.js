const allSideMenu = document.querySelectorAll('#sidebar .side-menu.top li a');

allSideMenu.forEach(item => {
    const li = item.parentElement;

    item.addEventListener('click', function () {
        allSideMenu.forEach(i => {
            i.parentElement.classList.remove('active');
        })
        li.classList.add('active');
    })
});


function applyTheme(theme) {
    if (theme === "dark") {
        document.body.classList.add('dark');
    } else {
        document.body.classList.remove('dark');
    }
}

// TOGGLE SIDEBAR
const menuBar = document.querySelector('#content nav .bx.bx-menu');
const sidebar = document.getElementById('sidebar');

menuBar.addEventListener('click', function () {
    sidebar.classList.toggle('hide');
})

if (window.innerWidth < 768) {
    sidebar.classList.add('hide');
}

const switchMode = document.getElementById('switch-mode');

// Load the theme from local storage
document.addEventListener("DOMContentLoaded", function () {
    const savedTheme = localStorage.getItem('theme') || 'light';
    applyTheme(savedTheme);
    switchMode.checked = (savedTheme === 'dark');
});

switchMode.addEventListener('change', function () {
    const theme = this.checked ? 'dark' : 'light';
    applyTheme(theme);
    localStorage.setItem('theme', theme);
});

function clearStorage() {
    localStorage.clear(); // Clear local storage
    sessionStorage.clear(); // Clear session storage if needed
}
function handleLogout(event) {
    event.preventDefault(); // Prevent default anchor behavior (navigation)

    // Clear local storage or session storage
    localStorage.clear();
    sessionStorage.clear();

    // Navigate to logout endpoint
    window.location.href = '/Users/Logout';
}

// Event listener for logout link click
document.getElementById('logout-link').addEventListener('click', handleLogout);

function clearStorage() {
        // Clear local storage
        localStorage.clear();
    // Clear session storage
    sessionStorage.clear();
}
