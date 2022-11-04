package groups.groupPermission.permission;

import org.springframework.http.HttpStatus;

import java.util.UUID;

public interface PermissionValidator {

    HttpStatus checkPermissionId(UUID permissionId);
}
