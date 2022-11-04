package groups.common.wrappers;

import groups.common.annotation.MustExist;

import java.util.UUID;

@MustExist(reason = MustExist.Reason.HIBERNATE)
public record UuidWrapper(UUID uuid) {
}
