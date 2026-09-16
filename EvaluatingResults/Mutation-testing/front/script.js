const form = document.getElementById('regForm');
const msg = document.getElementById('msg');
form.addEventListener('submit', async (ev) => {
  ev.preventDefault();
  msg.textContent = 'Enviando...';
  const data = Object.fromEntries(new FormData(form).entries());
  data.hasAcceptedTerms = form.elements.hasAcceptedTerms.checked;
  try {
    const res = await fetch('/api/register', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error(`${res.status} ${res.statusText}`);
    const { status } = await res.json();
    const messages = {
      Accepted: 'Registro aceptado. Gracias!',
      Waitlisted: 'El curso está completo. Te hemos añadido a la lista de espera.',
      Rejected: 'No podemos aceptar el registro sin la aceptación de los términos.'
    };
    msg.textContent = messages[status] ?? 'Registro enviado.';
    form.reset();
  } catch (err) {
    console.error(err);
    msg.textContent = 'Error al enviar';
  }
});
