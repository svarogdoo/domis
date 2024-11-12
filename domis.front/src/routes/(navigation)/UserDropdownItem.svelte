<script lang="ts">
  export let href: string;
  export let text: string;
</script>

<li>
  <a {href} class="w-full text-left block px-4 py-2 hover:bg-gray-100">
    {text}
  </a>
</li>
