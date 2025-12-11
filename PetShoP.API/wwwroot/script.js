// =====================
// MENÚ RESPONSIVE
// =====================
const menuToggle = document.getElementById('menuToggle');
const mainNav = document.getElementById('mainNav');

if (menuToggle && mainNav) {
    menuToggle.addEventListener('click', () => {
        mainNav.classList.toggle('show');
    });
}

// =====================
// FILTROS DE TIENDA
// =====================
const productsGrid = document.getElementById('productsGrid');
const filterCheckboxes = document.querySelectorAll('.filter-category');
const priceRange = document.getElementById('priceRange');
const priceRangeValue = document.getElementById('priceRangeValue');

function formatRD(value) {
    return `RD$ ${Number(value).toFixed(2)}`;
}

function applyShopFilters() {
    if (!productsGrid) return;

    const cards = productsGrid.querySelectorAll('.product-card');
    const activeCategories = new Set(
        Array.from(filterCheckboxes)
            .filter(cb => cb.checked)
            .map(cb => cb.value)
    );
    const maxPrice = priceRange ? parseFloat(priceRange.value) : Infinity;

    cards.forEach(card => {
        const category = card.dataset.category;
        const price = parseFloat(card.dataset.price || '0');
        const showByCategory =
            activeCategories.size === 0 || activeCategories.has(category);
        const showByPrice = price <= maxPrice;

        card.style.display = showByCategory && showByPrice ? '' : 'none';
    });
}

if (priceRange && priceRangeValue) {
    priceRangeValue.textContent = formatRD(priceRange.value);
    priceRange.addEventListener('input', () => {
        priceRangeValue.textContent = formatRD(priceRange.value);
        applyShopFilters();
    });
}

if (filterCheckboxes.length) {
    filterCheckboxes.forEach(cb =>
        cb.addEventListener('change', applyShopFilters)
    );
}

// Aplica filtros al cargar
applyShopFilters();

// =====================
// SELECCIÓN DE HORA (AGENDAR)
// =====================
const timeButtons = document.querySelectorAll('.time-slot');
const hiddenHourInput = document.getElementById('horaSeleccionada');

if (timeButtons.length && hiddenHourInput) {
    timeButtons.forEach(btn => {
        btn.addEventListener('click', () => {
            timeButtons.forEach(b => b.classList.remove('selected'));
            btn.classList.add('selected');
            hiddenHourInput.value = btn.dataset.time || btn.textContent.trim();
        });
    });
}

const bookingForm = document.getElementById('bookingForm');

if (bookingForm && hiddenHourInput) {
    bookingForm.addEventListener('submit', e => {
        if (!hiddenHourInput.value) {
            alert('Por favor, selecciona una hora para tu cita.');
            e.preventDefault();
        }
    });
}

// =====================
// CARRITO DE COMPRAS
// =====================
(function () {
    const productsGrid = document.getElementById('productsGrid');
    const cartItemsList = document.getElementById('cartItemsList');
    const cartEmpty = document.getElementById('cartEmpty');
    const cartTotalEl = document.getElementById('cartTotal');
    const cartCountBadge = document.getElementById('cartCountBadge');
    const cartClearBtn = document.getElementById('cartClear');

    if (!productsGrid || !cartItemsList || !cartTotalEl || !cartCountBadge) {
        return;
    }

    const STORAGE_KEY = 'petshop_cart';

    function loadCart() {
        try {
            const raw = localStorage.getItem(STORAGE_KEY);
            return raw ? JSON.parse(raw) : [];
        } catch (err) {
            console.error('Error al leer el carrito', err);
            return [];
        }
    }

    function saveCart(cart) {
        localStorage.setItem(STORAGE_KEY, JSON.stringify(cart));
    }

    let cart = loadCart();

    function renderCart() {
        cartItemsList.innerHTML = '';

        if (!cart.length) {
            cartEmpty.style.display = 'block';
        } else {
            cartEmpty.style.display = 'none';

            cart.forEach(item => {
                const li = document.createElement('li');
                li.className = 'cart-item';
                li.dataset.id = item.id;
                li.innerHTML = `
                    <div>
                        <div class="cart-item-name">${item.name}</div>
                        <div class="cart-item-meta">
                            RD$ ${item.price.toFixed(2)} × ${item.qty}
                        </div>
                    </div>
                    <div class="cart-item-actions">
                        <button type="button" data-action="minus">−</button>
                        <button type="button" data-action="plus">+</button>
                    </div>
                `;
                cartItemsList.appendChild(li);
            });
        }

        const total = cart.reduce((sum, it) => sum + it.price * it.qty, 0);
        cartTotalEl.textContent = total.toFixed(2);

        const count = cart.reduce((sum, it) => sum + it.qty, 0);
        cartCountBadge.textContent = count;
    }

    function addToCart(product) {
        const existing = cart.find(it => it.id === product.id);
        if (existing) {
            existing.qty += 1;
        } else {
            cart.push({ ...product, qty: 1 });
        }
        saveCart(cart);
        renderCart();
    }

    // Click en "Añadir al Carrito"
    productsGrid.addEventListener('click', e => {
        const btn = e.target.closest('.add-to-cart');
        if (!btn) return;

        const card = btn.closest('.product-card');
        if (!card) return;

        const id = card.dataset.id || card.querySelector('h3').textContent.trim();
        const name =
            (card.dataset.name || card.querySelector('h3').textContent).trim();
        const price = parseFloat(card.dataset.price || '0');

        if (!price) return;

        addToCart({ id, name, price });
    });

    // Botones + y − en el carrito
    cartItemsList.addEventListener('click', e => {
        const btn = e.target.closest('button[data-action]');
        if (!btn) return;

        const li = btn.closest('.cart-item');
        if (!li) return;

        const id = li.dataset.id;
        const item = cart.find(it => it.id === id);
        if (!item) return;

        if (btn.dataset.action === 'plus') {
            item.qty += 1;
        } else if (btn.dataset.action === 'minus') {
            item.qty -= 1;
            if (item.qty <= 0) {
                cart = cart.filter(it => it.id !== id);
            }
        }

        saveCart(cart);
        renderCart();
    });

    if (cartClearBtn) {
        cartClearBtn.addEventListener('click', () => {
            cart = [];
            saveCart(cart);
            renderCart();
        });
    }

    // Render inicial
    renderCart();
})();
