// -----------------------------------------------------------------------------
// script.js
// -----------------------------------------------------------------------------
// Submit handler for the interest form. It intercepts the form submission,
// serialises the fields to JSON, POSTs them to the API, and shows an inline
// message with the result.
// -----------------------------------------------------------------------------

// Cache the two elements we touch repeatedly.
const form = document.getElementById('regForm');
const msg = document.getElementById('msg');

// Handle the submit event instead of letting the browser navigate.
form.addEventListener('submit', async (ev) => {
  // Stop the default full-page form submission.
  ev.preventDefault();

  // Give immediate feedback while the request is in flight.
  msg.textContent = 'Enviando...';

  // Turn the form fields into a plain object: { name, email, course }.
  const data = Object.fromEntries(new FormData(form).entries());

  try {
    // Send the registration as JSON.
    const res = await fetch('/api/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });

    // fetch only rejects on network errors, so check the status explicitly and
    // treat any non-2xx as a failure.
    if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);

    // Success: confirm and clear the form for the next entry.
    msg.textContent = 'Registro enviado. Gracias!';
    form.reset();
  } catch (err) {
    // Log the real error for debugging, show a generic message to the visitor.
    console.error(err);
    msg.textContent = 'Error al enviar';
  }
});
