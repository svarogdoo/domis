<script lang="ts">
  export let user: AdminUser;
</script>

<tr class="hover:bg-domis-light cursor-pointer">
  <td class="w-40">{user.firstName ?? ""}</td>
  <td class="w-40">{user.lastName ?? ""}</td>
  <td class="w-40">{user.userName ?? ""}</td>
  <td class="w-16">{user.role ?? ""}</td>
  <!-- TODO: on hover show edit option -->
</tr>

<style>
  td {
    padding: 8px 12px;
  }
</style>
