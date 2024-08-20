<!-- +page.svelte -->
<ul class="w-64 bg-white rounded-lg shadow-lg">
  <!-- Category 1 -->
  <li class="border-b border-gray-200">
    <input type="checkbox" id="category1" class="peer hidden" />
    <label
      for="category1"
      class="block w-full px-4 py-2 text-left bg-gray-100 cursor-pointer peer-checked:bg-gray-200"
    >
      Category 1
    </label>
    <ul class="hidden peer-checked:block px-4 py-2 space-y-2">
      <li class="pl-4 text-gray-700 hover:text-gray-900">Subcategory 1.1</li>
      <li class="pl-4 text-gray-700 hover:text-gray-900">Subcategory 1.2</li>
    </ul>
  </li>

  <!-- Category 2 -->
  <li class="border-b border-gray-200">
    <input type="checkbox" id="category2" class="peer hidden" />
    <label
      for="category2"
      class="block w-full px-4 py-2 text-left bg-gray-100 cursor-pointer peer-checked:bg-gray-200"
    >
      Category 2
    </label>
    <ul class="hidden peer-checked:block px-4 py-2 space-y-2">
      <li class="pl-4 text-gray-700 hover:text-gray-900">Subcategory 2.1</li>
      <li class="pl-4 text-gray-700 hover:text-gray-900">Subcategory 2.2</li>
    </ul>
  </li>

  <!-- Category 3 -->
  <li class="border-b border-gray-200">
    <input type="checkbox" id="category3" class="peer hidden" />
    <label
      for="category3"
      class="block w-full px-4 py-2 text-left bg-gray-100 cursor-pointer peer-checked:bg-gray-200"
    >
      Category 3
    </label>
    <ul class="hidden peer-checked:block px-4 py-2 space-y-2">
      <li class="pl-4 text-gray-700 hover:text-gray-900">Subcategory 3.1</li>
      <li class="pl-4 text-gray-700 hover:text-gray-900">Subcategory 3.2</li>
    </ul>
  </li>
</ul>
