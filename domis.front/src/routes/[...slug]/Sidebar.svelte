<script lang="ts">
  import SidebarCategory from "./SidebarCategory.svelte";
  let categories: Array<Category> = [
    {
      name: "Nedeljna akcija",
      id: "/1",
    },
    {
      name: "Dekorativne letvice",
      id: "/2",
    },
    {
      name: "Podovi",
      id: "/3",
    },
    {
      name: "Pločice, graniti",
      id: "/9",
      subcategories: [
        {
          name: "Keramika",
          id: "/91",
          subcategories: [
            {
              name: "Jos",
              id: "/911",
              subcategories: [
                {
                  name: "I joos",
                  id: "/91111",
                },
                {
                  name: "Ide joos",
                  id: "/911112",
                },
              ],
            },
          ],
        },
        {
          name: "Graniti",
          id: "/92",
        },
      ],
    },
    {
      name: "Lajsne",
      id: "/4",
    },
    {
      name: "Graniti",
      id: "/5",
    },
    {
      name: "Rasveta",
      id: "/6",
    },
    {
      name: "Tepisi",
      id: "/7",
    },
    {
      name: "Garnišne",
      id: "/8",
    },
  ];
</script>

<aside class="w-full flex flex-col gap-y-4">
  <h2 class="text-xl">Svi proizvodi</h2>
  <ul class="flex flex-col border px-4 py-2 border-gray-400 rounded-md">
    {#each categories as category}
      <li
        class="w-full flex flex-col relative border-b border-gray-400 my-1 tracking-wider font-light"
      >
        <a href={category.id}>{category.name}</a>
        {#if category.subcategories}
          <SidebarCategory {category} />
        {/if}
      </li>
    {/each}
  </ul>
</aside>
