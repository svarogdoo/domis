<script lang="ts">
  export let user: AdminUser;
</script>

<tr>
  <td class="w-40">{user.firstName ?? ""}</td>
  <td class="w-40">{user.lastName ?? ""}</td>
  <td class="w-40">{user.userName ?? ""}</td>
  <td class="w-16">{user.roles[0] ?? ""}</td>
</tr>

<style>
  td {
    padding: 8px 12px;
  }
</style>
