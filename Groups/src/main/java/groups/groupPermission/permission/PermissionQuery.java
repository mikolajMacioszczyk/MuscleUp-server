package groups.groupPermission.permission;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface PermissionQuery {

    HttpStatus checkPermissionId(UUID permissionId);
}
