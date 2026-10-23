const form = document.getElementById('regForm');
const msg = document.getElementById('msg');
form.addEventListener('submit', async (ev) => {
  ev.preventDefault();
  msg.textContent = 'Enviando...';
  const data = Object.fromEntries(new FormData(form).entries());
  try {
    const res = await fetch('/api/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
    msg.textContent = 'Registro enviado. Gracias!';
    form.reset();
  } catch (err) {
    console.error(err);
    msg.textContent = 'Error al enviar';
  }
});